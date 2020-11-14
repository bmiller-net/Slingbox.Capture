using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Slingbox.Services;
using Slingbox.Services.Model;

namespace Slingbox.API.NetCore2
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var config = Configuration.Get<SlingboxConfiguration>();

            var ipAddress = IPAddress.Parse(config.Slingbox.IPAddress);
            var port = config.Slingbox.Port;
            var username = config.Slingbox.Username;
            var password = config.Slingbox.AdminPassword;

            services.Add(new ServiceDescriptor(typeof(VideoStream), v => new VideoStream(), ServiceLifetime.Singleton));
            services.Add(new ServiceDescriptor(typeof(SlingboxService), v => new SlingboxService(ipAddress, port, username, password), ServiceLifetime.Singleton));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

            //app.UseHttpsRedirection();
            app.UseMvc(r=> {
                r.MapRoute("DefaultApi", "api/{controller}/{id}");
            });
        }
    }
}
