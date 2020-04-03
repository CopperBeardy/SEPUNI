using System;
using System.ComponentModel.DataAnnotations;

namespace CordEstates.Areas.Staff.Models.DTOs
{
    public class CreateAppointmentDTO
    {

        public string StaffId { get; set; }
        public int ListingId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Time { get; set; }
    }
}
