using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageSharp;
using ImageSharp.Processing;
using LederfestPhoto.Configuration;
using Microsoft.AspNetCore.Mvc;
using LederfestPhoto.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LederfestPhoto.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    public class ChallengesController : Controller
    {
        private readonly LederfestPhotoContext _context;

        public ChallengesController(LederfestPhotoContext context)
        {
            _context = context;
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            List<Guid> used = await _context.Photos.Include(t => t.Team).Include(c => c.Challenge)
                .Where(p => p.Team.Id == id).Select(t => t.Challenge.Id).ToListAsync();
            var challenges = await _context.Challenges.Where(x => !used.Contains(x.Id) && x.ReleaseDateTime < DateTime.Now).ToListAsync();
            return Ok(challenges);
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var challenges = await _context.Challenges.ToListAsync();
            return Ok(challenges);
        }
        [HttpPost]
        
        public async Task<IActionResult> Post([FromBody]ChallengeInputModel input)
        {
            var challenge = new Challenge()
            {
                Id = new Guid(),
                Description = input.Text,
                ReleaseDateTime = input.ReleaseDate
            };
            _context.Add(challenge);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new {id = challenge.Id}, challenge);
        }



        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _context.Challenges.Remove(await _context.Challenges.Where(p => p.Id == id).FirstAsync());
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
