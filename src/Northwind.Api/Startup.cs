using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Api.Config;
using Northwind.Api.Exceptions.Handlers;
using Northwind.Api.Mapping;
using Northwind.Api.Middleware;
using Northwind.Core.UseCases.Products.GetAll;
using Northwind.Data;

namespace Northwind.Api
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
            services.AddMediatR(typeof(GetAllProductsQuery).Assembly);
            services.AddAutoMapper(typeof(MapperProfile).Assembly);

            services.Configure<ImageFileOptions>(Configuration.GetSection(ImageFileOptions.ImageFile));

            services.AddTransient<IExceptionHandler, EntityNotFoundExceptionHandler>();

            services.AddDbContext<NorthwindContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("Northwind"));
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
