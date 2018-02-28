using System.ComponentModel.DataAnnotations;

namespace LederfestPhoto.Models
{
    public class UpdateRatingInputModel
    {
        [Range(-1,10)]
        public int Rating { get; set; }
    }
}