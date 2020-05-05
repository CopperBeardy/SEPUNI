using CordEstates.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CordEstates.Models.DTOs
{
    public class CustomerListingDTO
    {
        public int Id { get; set; }
        public List<CustomerProperties> PropertiesInterestedIn { get; set; }
    }
}
