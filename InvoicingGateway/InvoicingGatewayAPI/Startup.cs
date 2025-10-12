using Application;
using Application.Utils.ModelConverter;
using Infrastructure;
using Infrastructure.Context;
using Microsoft.AspNet.OData.Extensions;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicingGatewayAPI
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

            //services.AddControllers().AddNewtonsoftJson();
            services.AddDbContext<InvoicingContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("ConnectionString")));
            services.AddRepositortyServices();
            services.AddApplicationServices();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "InvoicingGatewayAPI", Version = "v1" });
            });
            
            services.AddControllers(mvcOptions =>
            mvcOptions.EnableEndpointRouting = false).AddNewtonsoftJson();
            //services.AddDbContext<InvoicingContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("ConnectionString")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
            // register Cors service on DI container
            services.AddCors();
            //Add OData
            services.AddOData();
            //Health check
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "InvoicingGatewayAPI v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(x => x
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowAnyOrigin()
                  .WithExposedHeaders("X-Pagination")
                   );

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
                endpoints.EnableDependencyInjection();
                endpoints.Select().Count().Filter().OrderBy().MaxTop(100).SkipToken().Expand();
            });
            /*app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllers();
            });*/
        }
    }
}
