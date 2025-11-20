using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Order.Microservice.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Order.Microservice
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection"),
                   sqlServerOptions => 
                   {
                       sqlServerOptions.EnableRetryOnFailure(
                           maxRetryCount: 10,
                           maxRetryDelay: TimeSpan.FromSeconds(30),
                           errorNumbersToAdd: null);
                       sqlServerOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                   }));
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            #region Swagger
            services.AddSwaggerGen(c =>
            {
                var xmlFile = "Order.Microservice.xml";
                var xmlPath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, xmlFile);
                if (System.IO.File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Order Microservice API",
                });
            });
            #endregion
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Global exception handler
            app.UseMiddleware<Middleware.GlobalExceptionHandler>();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order.Microservice");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
