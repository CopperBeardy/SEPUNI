using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CordEstates.Entities
{
[ExcludeFromCodeCoverage]
    public class Address
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        public string FirstLine { get; set; }

        public string SecondLine { get; set; }
        [Required]
        public string TownCity { get; set; }
        [Required]
        public string Postcode { get; set; }





    }
}
