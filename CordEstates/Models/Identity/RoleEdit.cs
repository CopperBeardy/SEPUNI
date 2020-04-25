using CordEstates.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CordEstates.Models.Identity
{
    public class RoleEdit
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<ApplicationUser> Members { get; set; }
        public IEnumerable<ApplicationUser> NonMembers { get; set; }
    }
}
