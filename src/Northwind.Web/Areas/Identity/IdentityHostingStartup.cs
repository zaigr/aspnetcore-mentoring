using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Northwind.Web.Areas.Identity.Data;
using Northwind.Web.Areas.Identity.Services;
using Northwind.Web.Configuration;
using Northwind.Web.Const;

[assembly: HostingStartup(typeof(Northwind.Web.Areas.Identity.IdentityHostingStartup))]
namespace Northwind.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("Northwind.Identity")));

                services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<IdentityContext>();

                services
                    .AddAuthentication()
                    .AddMicrosoftIdentityWebApp(
                        context.Configuration, 
                        subscribeToOpenIdConnectMiddlewareDiagnosticsEvents: true);

                services.AddAuthorization();

                services.AddRazorPages()
                    .AddMicrosoftIdentityUI();

                services.AddTransient<IEmailSender, EmailSender>();
                services.Configure<SendGridOptions>(context.Configuration.GetSection(SendGridOptions.SendGrid));
            });
        }
    }
}