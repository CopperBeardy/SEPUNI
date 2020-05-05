using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CordEstates.Entities
{
    public class CustomerProperties
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public Listing Listing { get; set; }

        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        [ForeignKey(nameof(Listing))]
        public int PropertyId { get; set; }

    }
}
