using AutoMapper;
using Ct.Interview.Common.Helpers;
using Ct.Interview.Data.Models;
using Ct.Interview.Repository;
using Ct.Interview.Repository.FileHandlers;
using Ct.Interview.Repository.Interfaces;
using Ct.Interview.Web.Api.HostedServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace Ct.Interview.Web.Api
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
            services.AddDbContext<CtInterviewDBContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString(name: "DefaultConnection")));

            services.AddScoped(serviceType: typeof(IUnitOfWork), implementationType: typeof(UnitOfWork));
            services.AddScoped(typeof(IRepository<>), implementationType: typeof(GenericRepository<>));
            services.AddTransient(typeof(ICsvHandler), implementationType: typeof(CsvHandler));

            services.AddAutoMapper(typeof(Startup));
            services.AddHostedService<ImportAsxFileHostedService>();

            services.AddResponseCaching();

            services.AddMvc(options => 
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute())
                ).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            
            services.AddSwaggerGen(options =>
                options.SwaggerDoc("v1", new Info { Title = "Ct.Interview", Version = "v1" })
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Ct Interview v1"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseResponseCaching();            
            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                    new CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(
                            Configuration.GetSection("CacheExpirationInSeconds")
                            .Value.CacheExpirationToDouble())
                    };
                context.Response.Headers[HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };

                await next();
            });

            app.UseHttpsRedirection();
            app.UseMvc();
            
        }
    }
}
