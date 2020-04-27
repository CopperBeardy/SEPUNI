using CordEstates.Areas.Identity.Data;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CordEstates.Entities
{
    public class Customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public List<Listing> PropertiesInterestedIn { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
    }
}
