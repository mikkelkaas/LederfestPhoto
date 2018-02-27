using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace LederfestPhoto.Models
{
    public class PhotoInputModel
    {
        public Guid Team { get; set; }
        public string Text { get; set; }
        public IList<IFormFile> Photos { get; set; }
        public Guid Challenge { get; set; }

    }
}