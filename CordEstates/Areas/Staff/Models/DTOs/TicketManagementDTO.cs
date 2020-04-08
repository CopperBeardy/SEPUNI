using System;

namespace CordEstates.Areas.Staff.Models.DTOs
{
    public class TicketManagementDTO
    {
        public string Id { get; set; }


        public string FirstName { get; set; }


        public string LastName { get; set; }


        public string Email { get; set; }


        public string Message { get; set; }

        public DateTime SentAt { get; set; }

        public bool Actioned { get; set; }

    }
}
