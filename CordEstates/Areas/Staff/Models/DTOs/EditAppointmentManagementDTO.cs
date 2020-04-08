using System;

namespace CordEstates.Areas.Staff.Models.DTOs
{
    public class EditAppointmentManagementDTO
    {

        public int Id { get; set; }


        public string StaffId { get; set; }

        public int ListingId { get; set; }

        public DateTime Time { get; set; }
    }
}
