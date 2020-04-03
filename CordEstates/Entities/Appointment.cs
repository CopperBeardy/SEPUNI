using CordEstates.Areas.Identity.Data;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CordEstates.Entities
{[ExcludeFromCodeCoverage]
    public class Appointment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public ApplicationUser Staff { get; set; }


        [Required]
        public DateTime Time { get; set; }

        [ForeignKey(nameof(Staff))]
        public string StaffId { get; set; }
        [Required]
        public Listing Listing { get; set; }


        [ForeignKey(nameof(Listing))]
        public int ListingId { get; set; }

    }
}
