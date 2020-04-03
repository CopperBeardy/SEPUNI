using CordEstates.Entities;

namespace CordEstates.Models.DTOs
{
    public class ExtendedListingDTO
    {
        public int Id { get; set; }
        public Address Address { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Url { get; set; }


        public override string ToString()
        {

            return $"{Address.Number}, {Address.FirstLine}, {Address.TownCity}, {Address.Postcode}";
        }
    }
}
