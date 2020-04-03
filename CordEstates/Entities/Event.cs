using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CordEstates.Entities
{
    [ExcludeFromCodeCoverage]
    public class Event
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Event Name must be shorter than 50 characters")]
        public string EventName { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Details must be shorter than 500 characters")]
        public string Details { get; set; }

        [Required]
        public Photo Photo { get; set; }
        [Required]
        public DateTime Time { get; set; }
        public bool Active { get; set; }

        [ForeignKey(nameof(Photo))]
        public int PhotoId { get; set; }
    }
}
