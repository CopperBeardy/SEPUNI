using CordEstates.Entities;
using CordEstates.Models.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace CordEstates.Areas.Staff.Models.DTOs
{
    public class ListingManagementDTO
    {


        public int Id { get; set; }
        public Address Address { get; set; }
        public int AddressId { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public SaleStatus Status { get; set; }
        public Photo Image { get; set; }


        [NotMapped]
        public IFormFile File { get; set; }

        public override string ToString()
        {

            return $"{Address.Number}, {Address.FirstLine}, {Address.TownCity}, {Address.Postcode}";
        }
    }
}
