using CordEstates.Entities;
using Microsoft.AspNetCore.Identity;

namespace CordEstates.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }
        [PersonalData]
        public Photo HeadShot { get; set; }
        [PersonalData]
        public string Bio { get; set; }


    }
}
