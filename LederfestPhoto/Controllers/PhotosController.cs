using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LederfestPhoto.Models;
using Microsoft.EntityFrameworkCore;

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

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PhotoInputModel input)
        {
            var photo = new Photo()
            {
                Team = input.Team,
                Text = input.Text,
                Challenge = new Challenge()

            };
            return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
