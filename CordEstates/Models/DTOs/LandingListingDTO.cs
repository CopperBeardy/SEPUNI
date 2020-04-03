
using System;

namespace CordEstates.Models.DTOs
{
    public class LandingListingDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public Uri ImageUrl { get; set; }
    }
}
