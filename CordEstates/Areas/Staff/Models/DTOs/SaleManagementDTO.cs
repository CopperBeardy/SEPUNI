using CordEstates.Areas.Identity.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CordEstates.Entities;
using CordEstates.Models.Enums;

namespace CordEstates.Areas.Staff.Models.DTOs
{
    public class SaleManagementDTO
    {     
        public int Id { get; set; }
        public DateTime DateOfSale { get; set; }
        public Buyer Buyer { get; set; }
        public double AgreedPrice { get; set; }  
        public ApplicationUser SellingAgent { get; set; }
        public Address SoldProperty { get; set; }
        public int BuyerId { get; set; }
        public string AgentId { get; set; }    
        public int PropertyId { get; set; }
        [NotMapped]
        public SaleStatus Status { get; set; }
    }
}
