using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExchangeSync.Model;
using ExchangeSync.Model.Consumers;
using ExchangeSync.Services;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeSync
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddHttpClient<IIdentityService, IdentityService>();
            services.AddHttpContextAccessor();
            services.AddDbContext<ServiceDbContext>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IServerRenderService, ServerRenderService>(u => new ServerRenderService("./server"));
            services.AddScoped<IMailService, MailService>();

            var builder = new DbContextOptionsBuilder<ServiceDbContext>()
                //.UseLazyLoadingProxies()
                .UseSqlServer("");
            var dbOptions = builder.Options;
            //IMapper mapper = null;
            //Bus.Factory.CreateUsingRabbitMq(cfg =>
            //{
            //    var host = cfg.Host(new Uri(""), h =>
            //    {
            //        h.Username("");
            //        h.Password("");
            //    });
            //    cfg.ReceiveEndpoint(host, "ExchangeSync", c =>
            //    {
            //        c.Consumer(() => new MdmDataConsumer(dbOptions, mapper));
            //        c.Consumer(() => new OrgEventDataConsumer(dbOptions, mapper));
            //    });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "user",
                    template: "user/{keyword?}",
                    defaults: new { controller = "User", action = "GetUser" }
                    );
                routes.MapRoute(
                    name: "catch-all",
                    template: "{*url}",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
