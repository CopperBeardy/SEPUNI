using CordEstates.Areas.Identity.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CordEstates.Entities
{
    public class Sale
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime DateOfSale { get; set; }

        [Required]
        public Buyer Buyer { get; set; }

        [Required]
        public double AgreedPrice { get; set; }

        [Required]
        public ApplicationUser SellingAgent { get; set; }

        [Required]
        public Address SoldProperty { get; set; }

        [ForeignKey(nameof(Buyer))]
        public int BuyerId { get; set; }

        [ForeignKey(nameof(SellingAgent))]
        public string AgentId { get; set; }

        [ForeignKey(nameof(SoldProperty))]
        public int PropertyId { get; set; }
    }
}
