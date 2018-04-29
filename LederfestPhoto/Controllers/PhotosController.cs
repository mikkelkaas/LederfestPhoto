using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageSharp;
using ImageSharp.Processing;
using LederfestPhoto.Configuration;
using LederfestPhoto.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

namespace LederfestPhoto.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    public class PhotosController : Controller
    {
        private readonly ConnectionStrings _connectionStrings;
        private readonly LederfestPhotoContext _context;

        public PhotosController(LederfestPhotoContext context, IOptions<ConnectionStrings> connectionStrings)
        {
            _context = context;
            _connectionStrings = connectionStrings.Value;
        }

        // GET api/values
        [HttpGet]
        [Route("GetSingle")]
        public async Task<IActionResult> GetSingle(bool excludenotrated = true)
        {
            return Ok(excludenotrated
                ? await _context.Photos.Include(i => i.Team).Include(i => i.Challenge).Where(r => r.Rating > -1)
                    .OrderBy(r => Guid.NewGuid()).FirstOrDefaultAsync()
                : await _context.Photos.Include(i => i.Team).Include(i => i.Challenge).OrderBy(r => Guid.NewGuid())
                    .FirstOrDefaultAsync());
        }


        // GET api/values
        [HttpGet]
        [Route("GetUnrated")]
        public async Task<IActionResult> GetUnrated()
        {
            return Ok(await _context.Photos.Include(i => i.Team).Include(i => i.Challenge).Where(r => r.Rating == -1)
                .OrderBy(r => Guid.NewGuid()).FirstOrDefaultAsync());
        }


        // GET api/values
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll(bool excludenotrated = true)
        {
            return Ok(excludenotrated
                ? await _context.Photos.Where(r => r.Rating > -1).Include(i => i.Team).Include(i => i.Challenge)
                    .ToListAsync()
                : await _context.Photos.Include(i => i.Team).Include(i => i.Challenge).ToListAsync());
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Post(PhotoInputModel input)
        {
            var challenge = await _context.Challenges.Where(c => c.Id == input.Challenge).FirstOrDefaultAsync();
            if (challenge == null)
                return BadRequest("Challenge not found");
            Team team = await _context.Teams.Where(t => t.Id == input.Team).FirstOrDefaultAsync();
            if (team == null)
                return BadRequest("Team not found");
        
            var guid = Guid.NewGuid();

            var photo = new Photo
            {
                Team = team,
                //Text = input.Text,
                Challenge = challenge,
                Id = guid
            };
            if (_context.Photos.Include(t => t.Team).Include(c => c.Challenge)
                    .Count(t => t.Team.Id == team.Id && t.Challenge.Id == challenge.Id) > 0)
            {
                return BadRequest("Allready posted");
            }
            if (ModelState.IsValid)
            {
                var filePathPhoto = Path.GetTempFileName();

                using (var stream = new FileStream(filePathPhoto, FileMode.Create))
                {
                    await input.Photo.CopyToAsync(stream);
                }

                //var filePathPhotoResized = ResizeImage(filePathPhoto);

                var photoResizedBlobPath = await uploadToAzureBlob(guid, filePathPhoto);

                photo.BlobPath = photoResizedBlobPath;
                _context.Add(photo);
                await _context.SaveChangesAsync();

                System.IO.File.Delete(filePathPhoto);
                //System.IO.File.Delete(filePathPhotoResized);
            }
            return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] UpdateRatingInputModel input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var photo = await _context.Photos.Where(p => p.Id == id).FirstAsync();
            photo.Rating = input.Rating;
            photo.Rotation = input.Rotation;
            await _context.SaveChangesAsync();
            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _context.Photos.Remove(await _context.Photos.Where(p => p.Id == id).FirstAsync());
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet, Route("Results")]
        public async Task<IActionResult> Results()
        {
            var scoreResults = await _context.Photos.Include(t => t.Team).GroupBy(t => t.Team).Select(g => new ScoreResult(g)).ToListAsync();
            return Ok(scoreResults.OrderByDescending(r => r.Score));
        }

        private static string ResizeImage(string inputPath)
        {
            var outputPath = inputPath + "Resized";
            using (var imageToResize = System.IO.File.OpenRead(inputPath))
            {
                using (var output = System.IO.File.OpenWrite(outputPath))
                {
                    var image = new Image(imageToResize).Resize(new ResizeOptions
                    {
                        Size = new Size(1000, 1000),

                        Mode = ResizeMode.Max
                    });
                    image.Save(output);
                }
            }
            return outputPath;
        }

        private async Task<string> uploadToAzureBlob(Guid guid, string filePath)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_connectionStrings.AzureCloudStorage);
            
            // Create a blob client.
            var blobClient = storageAccount.CreateCloudBlobClient();

            // Get a reference to a container named "mycontainer."
            var container = blobClient.GetContainerReference("images");
            var documentName = guid.ToString();

            var blockBlob = container.GetBlockBlobReference(documentName);

            // Create or overwrite the "myblob" blob with the contents of a local file
            // named "myfile".
            using (var fileStream = System.IO.File.OpenRead(filePath))
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
            }
            return blockBlob.Uri.ToString();
        }
    }
}