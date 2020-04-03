using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CordEstates.Entities
{
    public class Ticket
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Surname")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "Message must be between 15 and 300 characters", MinimumLength = 15)]
        public string Message { get; set; }


        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public bool Actioned { get; set; } = false;
    }
}
