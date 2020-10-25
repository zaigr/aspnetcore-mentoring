using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Northwind.Core.UseCases.Categories.GetAll;
using Northwind.Data;
using Northwind.Web.Configuration;
using Northwind.Web.Mapping;
using Serilog;

namespace Northwind.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(
                    "Northwind.Web.log",
                    fileSizeLimitBytes: 5242880,
                    retainedFileCountLimit: 15,
                    rollOnFileSizeLimit: true)
                .CreateLogger();

            services.AddControllersWithViews();

            services.AddDbContext<NorthwindContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("Northwind"));
            });

            services.AddMediatR(typeof(GetAllCategoriesQuery).Assembly);

            services.AddAutoMapper(typeof(MapperProfile).Assembly);

            services.Decorate<IConfiguration, ConfigurationLogger>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation("Middleware configuration started.");

            if (env.IsDevelopment())
            {
                logger.LogInformation("In Development.");

                app.UseDeveloperExceptionPage();
            }
            else
            {
                logger.LogInformation("Not Development.");

                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            logger.LogInformation("Middleware configuration finished.");
        }
    }
}
