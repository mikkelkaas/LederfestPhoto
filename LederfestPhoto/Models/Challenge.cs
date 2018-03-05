using System;

namespace LederfestPhoto.Models
{
    public class Challenge
    {
        public Guid Id { get; set; }
        public string Description { get; set; }

        public DateTime ReleaseDateTime { get; set; } = DateTime.Now;
    }
}