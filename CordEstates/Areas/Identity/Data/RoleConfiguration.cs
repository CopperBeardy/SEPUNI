using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CordEstates.Areas.Identity.Data
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(

             new IdentityRole
             {
                 Name = "Staff",
                 NormalizedName = "STAFF"
             },
            new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "Admin"
            });
        }
    }
}
