using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pharmacies.Server.Data;

[assembly: HostingStartup(typeof(Pharmacies.Server.Areas.Identity.IdentityHostingStartup))]
namespace Pharmacies.Server.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<PharmaciesServerContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("PharmaciesServerContextConnection")));

                services.AddDefaultIdentity<Pharmacies.Server.Data.ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<PharmaciesServerContext>();
            });
        }
    }
}