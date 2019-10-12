using AutoMapper;
using Ct.Interview.Data.Models;
using Ct.Interview.Repository;
using Ct.Interview.Repository.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddAutoMapper(typeof(Startup));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(options =>
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Ct.Interview", Version = "v1" })
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Ct Interview v1")
            );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
