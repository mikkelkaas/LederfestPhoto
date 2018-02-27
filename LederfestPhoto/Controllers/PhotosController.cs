using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageSharp;
using ImageSharp.Processing;
using Microsoft.AspNetCore.Mvc;
using LederfestPhoto.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LederfestPhoto.Controllers
{
    [Route("api/[controller]")]
    public class PhotosController : Controller
    {
        private readonly LederfestPhotoContext _context;

        protected PhotosController(LederfestPhotoContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get([FromRoute]bool excludeNotRated = true)
        {
            return Ok(excludeNotRated ? await _context.Photos.Where(r => r.Rating > -1).OrderBy(r => Guid.NewGuid()).FirstOrDefaultAsync() : await _context.Photos.OrderBy(r => Guid.NewGuid()).FirstOrDefaultAsync());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PhotoInputModel input)
        {
            var challenge = await _context.Challenges.Where(c => c.Id == input.Challenge).FirstAsync();
            var guid = Guid.NewGuid();

            var photo = new Photo()
            {
                Team = input.Team,
                Text = input.Text,
                Challenge = challenge,
                Id = guid
            };
            if (ModelState.IsValid)
            {

                var photos = input.Photos;
                

                // full path to file in temp location
                var filePathPhoto = Path.GetTempFileName();

                foreach (var formFile in photos)
                {
                    if (formFile.Length > 0)
                    {
                        using (var stream = new FileStream(filePathPhoto, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
                }



                var filePathQuestionResized = ResizeImage(filePathPhoto);

                var photoResizedBlobPath = await uploadToAzureBlob(guid, filePathQuestionResized);

                photo.BlobPath = photoResizedBlobPath;
                


                
                _context.Add(photo);
                await _context.SaveChangesAsync();

                System.IO.File.Delete(filePathPhoto);
            }
            return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody]int rating)
        {
            var photo = await _context.Photos.Where(p => p.Id == id).FirstAsync();
            photo.Rating = rating;
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

        private static string ResizeImage(string inputPath)
        {
            var outputPath = inputPath + "Resized";
            using (var imageToResize = System.IO.File.OpenRead(inputPath))
            {
                using (var output = System.IO.File.OpenWrite(outputPath))
                {
                    var image = new Image(imageToResize).Resize(new ResizeOptions
                        {
                            Size = new ImageSharp.Size(1000, 1000),

                            Mode = ResizeMode.Max
                        });
                    image.Save(output);
                }
            }
            return outputPath;
        }

        private static async Task<string> uploadToAzureBlob(Guid guid, string filePath)
        {
            CloudStorageAccount storageAccount = new CloudStorageAccount(
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
                    "",
                    ""), true);

            // Create a blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Get a reference to a container named "mycontainer."
            CloudBlobContainer container = blobClient.GetContainerReference("images");
            var documentName = guid.ToString();

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(documentName);

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
