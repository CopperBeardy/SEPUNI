using CordEstates.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CordEstates.Areas.Staff.Models.DTOs
{
    public class UserManagementDTO
    {

        public string Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Photo HeadShot { get; set; }
        public string Bio { get; set; }


        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }


        [NotMapped]
        public string Password { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
    }
}
