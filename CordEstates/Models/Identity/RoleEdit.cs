using CordEstates.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CordEstates.Models.Identity
{
    public class RoleEdit
    {
        
            public IdentityRole Role { get; set; }
            public IEnumerable<ApplicationUser> Members { get; set; }
            public IEnumerable<ApplicationUser> NonMembers { get; set; }
        
    }
}
