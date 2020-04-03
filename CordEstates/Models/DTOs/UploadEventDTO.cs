using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CordEstates.Models.DTOs
{
    public class UploadEventDTO
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string Details { get; set; }
        public Uri ImagePath { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }

    
        public DateTime Time { get; set; }
        public bool Active { get; set; }


    }
}
