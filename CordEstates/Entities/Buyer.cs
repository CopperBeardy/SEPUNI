using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CordEstates.Entities
{
    public class Buyer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        public int HouseNumber { get; set; }
        [Required]
        public string FirstLine { get; set; }
        [Required]
        public string Postcode { get; set; }
        [Required]
        public string PhoneNumber { get; set; }


    }
}