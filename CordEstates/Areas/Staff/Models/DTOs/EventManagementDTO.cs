using CordEstates.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CordEstates.Areas.Staff.Models.DTOs
{
    public class EventManagementDTO
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string Details { get; set; }
        public Photo Photo { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
        public DateTime Time { get; set; }
        public bool Active { get; set; }


    }
}
