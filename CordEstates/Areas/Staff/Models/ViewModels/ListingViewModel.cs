using CordEstates.Entities;
using CordEstates.Models.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CordEstates.Areas.Staff.Models.ViewModels
{
    public class ListingViewModel
    {
        public int Id { get; set; }
        public int AddressId { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public SaleStatus Status { get; set; } = SaleStatus.ForSale;
        public Photo Image { get; set; }

       


    }
}
