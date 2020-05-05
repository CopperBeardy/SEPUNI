using CordEstates.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace CordEstates.Models.DTOs
{
    public class ListingDetailDTO
    {
        public int Id { get; set; }
        public Address Address { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Url { get; set; }

        [NotMapped]
        public bool Follow { get; set; }
        
    }
}
