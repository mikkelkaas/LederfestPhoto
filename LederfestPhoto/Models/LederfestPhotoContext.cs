using Microsoft.EntityFrameworkCore;

namespace LederfestPhoto.Models
{
    public class LederfestPhotoContext : DbContext
    {
        public LederfestPhotoContext(DbContextOptions<LederfestPhotoContext> options)
            : base(options)
        {
        }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
    }
}