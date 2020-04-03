using CordEstates.Entities;
using System;

namespace CordEstates.Areas.Staff.Models.DTOs
{
    public class AppointmentDTO 
    {

        public int Id { get; set; }


        public string StaffId { get; set; }

        public Listing Listing { get; set; }

        public DateTime Time { get; set; }

       
    }
}
