using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(CordEstates.Areas.Identity.IdentityHostingStartup))]
namespace CordEstates.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            //builder.ConfigureServices((context, services) =>
            //{
            //    services.AddDbContext<ApplicationDbContext>(options =>
            //        options.UseSqlServer(
            //            context.Configuration.GetConnectionString("ApplicationDbContextConnection")));

            //    services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //        .AddEntityFrameworkStores<ApplicationDbContext>();
            //});
        }
    }
}