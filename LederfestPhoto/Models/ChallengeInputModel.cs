using System;

namespace LederfestPhoto.Models
{
    public class ChallengeInputModel
    {
        public string Text { get; set; }
        public DateTime ReleaseDate { get; set; } = DateTime.Now;
    }
}