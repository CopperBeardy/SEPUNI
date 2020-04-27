using CordEstates.Areas.Identity.Data;
using CordEstates.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CordEstates.Areas.Staff.Models.DTOs
{
    public class CustomerManagementDTO
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public  List<Listing> PropertiesInterestedIn { get; set; }    

        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
    }
}
