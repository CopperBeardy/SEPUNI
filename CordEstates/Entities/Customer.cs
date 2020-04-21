using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CordEstates.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public virtual List<Listing> PropertiesInterestedIn { get; set; }
    }
}
