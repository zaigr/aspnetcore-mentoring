using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
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
            services.AddRouting(c => c.LowercaseUrls = true);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Northwind API",
                    Description = "REST API used to manage Northwind products.",
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(Configuration.GetSection("AllowedOrigins").Get<string[]>());
                    });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Northwind API");
            });

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
