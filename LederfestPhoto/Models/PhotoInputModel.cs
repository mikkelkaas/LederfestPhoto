using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace LederfestPhoto.Models
{
    public class PhotoInputModel
    {
        public Guid Team { get; set; }
        public string TeamName { get; set; } = null;
        public string Text { get; set; }
        public IFormFile Photo { get; set; }
        public Guid Challenge { get; set; }

    }
}