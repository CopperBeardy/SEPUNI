using CordEstates.Models.DTOs;
using System.Collections.Generic;

namespace CordEstates.Models.ViewModels
{
    public class LandingPageViewModel
    {
        public List<ServiceDTO> Services { get; set; }
        public List<LandingListingDTO> Listings { get; set; }

        public EventDTO Events { get; set; }
    }
}
