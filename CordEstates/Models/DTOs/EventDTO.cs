using CordEstates.Entities;
using System;

namespace CordEstates.Models.DTOs
{
    public class EventDTO
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string Details { get; set; }
        public Uri ImagePath { get; set; }
        public Photo Photo { get; set; }
        public DateTime Time { get; set; }
        public bool Active { get; set; }


    }
}
