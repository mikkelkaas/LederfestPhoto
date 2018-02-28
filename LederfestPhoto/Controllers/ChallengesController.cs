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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LederfestPhoto.Controllers
{
    [Route("api/[controller]")]
    public class ChallengesController : Controller
    {
        private readonly LederfestPhotoContext _context;

        public ChallengesController(LederfestPhotoContext context)
        {
            _context = context;
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get([FromRoute]Guid id)
        {
            var challenge = await _context.Challenges.FindAsync(id);
            return Ok(challenge);
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
                Description = input.Text
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
