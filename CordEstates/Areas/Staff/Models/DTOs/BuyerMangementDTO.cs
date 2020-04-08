using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CordEstates.Areas.Staff.Models.DTOs
{
    public class BuyerManagementDTO
    {
        public int Id { get; set; }


        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int HouseNumber { get; set; }

        public string FirstLine { get; set; }
  
        public string Postcode { get; set; }

        public string PhoneNumber { get; set; }


    }
}