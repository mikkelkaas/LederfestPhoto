using System;
using System.ComponentModel.DataAnnotations;

namespace LederfestPhoto.Models
{
    public class Photo
    {
        public Guid Id { get; set; }
        public Guid Team { get; set; }
        public string Text { get; set; }
        public Challenge Challenge { get; set; }

        [Range(-1, 10)]
        public int Rating { get; set; } = -1;
    }
}