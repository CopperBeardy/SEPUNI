using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CordEstates.Areas.Staff.Models.DTOs
{
    public class UserManagementDTO
    {

        public string Id { get; set; }   
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HeadShotUrl { get; set; }
        public string Bio { get; set; }
      
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required]
        [NotMapped]
        public string Password { get; set; }
    }
}
