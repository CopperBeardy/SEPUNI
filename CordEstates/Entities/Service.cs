using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CordEstates.Entities
{
    public class Service
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "Service names must be between 10 and 25 characters long", MinimumLength = 10)]
        public string ServiceName { get; set; }


        [Required]
        [StringLength(500, ErrorMessage = "Description must be between 30 and 500 characters long", MinimumLength = 30)]
        public string Description { get; set; }
    }
}
