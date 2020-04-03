using CordEstates.Entities;

namespace CordEstates.Models.DTOs
{
    public class ListingDTO
    {
        public int Id { get; set; }
        public Address Address { get; set; }

        public double Price { get; set; }
        public string Url { get; set; }
    }
}
