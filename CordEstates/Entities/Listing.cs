
using CordEstates.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CordEstates.Entities
{
    public class Listing
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

       
        public Address Address { get; set; }


        [Required]
        [StringLength(300, ErrorMessage = "Description must be between 30 and 300 characters long", MinimumLength = 30)]
        public string Description { get; set; }


        [Required]
        public double Price { get; set; }

        [Required]
        public SaleStatus Status { get; set; }

        public Photo Image { get; set; }

        [ForeignKey(nameof(Address))]
        public int AddressId { get; set; }

        [ForeignKey(nameof(Image))]
        public int ImageId { get; set; }


        public override string ToString()
        {

            return $"{Address.Number}, {Address.FirstLine}, {Address.TownCity}, {Address.Postcode}";
        }

    }
}
