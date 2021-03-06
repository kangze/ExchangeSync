﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ExchangeSync.Extension;
using ExchangeSync.Model;
using ExchangeSync.Model.Consumers;
using ExchangeSync.Model.Services;
using ExchangeSync.Services;
using ExchangeSync.Services.Dtos;
using ExchangeSync.Skype;
using MassTransit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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
            services.AddDataProtection()
                .PersistKeysToFileSystem(new System.IO.DirectoryInfo(@"C:\dataprotection\"))
                .SetApplicationName("Mail")
                .SetDefaultKeyLifetime(TimeSpan.FromDays(365));
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });
            services.AddHttpClient<IIdentityService, IdentityService>();
            services.AddHttpContextAccessor();
            var databseSection = Configuration.GetSection("Database");
            var mdmBusSection = Configuration.GetSection("MdmBus");
            var idSvrSection = Configuration.GetSection("IdSvr");
            var serverConfig = Configuration.GetSection("ServerConfig");
            services.Configure<IdSvrOption>(idSvrSection);
            var idSvrOption = services.BuildServiceProvider().GetService<IOptions<IdSvrOption>>();
            services.AddDbContext<ServiceDbContext>(option =>
            {
                option.UseSqlServer(databseSection.GetValue<string>("ConnectString"));
            });
            //services.AddNofiyTask(serverConfig.GetValue<int>("Index"));
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.AddMvc()
                .AddJsonOptions(json => { json.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IServerRenderService, ServerRenderService>(u => new ServerRenderService("./server.js"));
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ICalendarService, CalendarService>();
            services.AddScoped<IMeetingService, MeetingService>();
            services.AddScoped<IOaSystemOperationService, OaSystemOperationService>();
            services.AddHttpClient<IOaSystemOperationService, OaSystemOperationService>();
            services.AddHttpClient<IMeetingService, MeetingService>();
            services.AddScoped<SkypeBootstraper>(u => new SkypeBootstraper(new HttpClient(), new SkypeOption()
            {
                DiscoverServer = "http://lyncdiscoverinternal.scrbg.com/"
            }));
            
            services.AddScoped<IEnterpriseContactService, EnterpriseContactService>();
            var builder = new DbContextOptionsBuilder<ServiceDbContext>()
                //.UseLazyLoadingProxies()
                .UseSqlServer(databseSection.GetValue<string>("ConnectString"));
            var dbOptions = builder.Options;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSingleton(new OaApiOption()
            {
                CreateAppointment = "https://newoa.scrbg.com/api/services/app/wxapi/SendMeetNoticeMsg",
            });
            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
                options.ValueLengthLimit = int.MaxValue;
                options.KeyLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MultipartBoundaryLengthLimit = int.MaxValue;
            });
            //var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            // {
            //     var host = cfg.Host(new Uri(mdmBusSection.GetValue<string>("Host")), h =>
            //     {
            //         h.Username(mdmBusSection.GetValue<string>("UserName"));
            //         h.Password(mdmBusSection.GetValue<string>("Password"));
            //     });
            //     cfg.ReceiveEndpoint(host, "ITSCRBG_Contact_Contact_2", c =>
            //     {
            //         c.Consumer(() => new MdmDataConsumer(dbOptions, mapper));
            //         c.Consumer(() => new OrgEventDataConsumer(dbOptions, mapper));
            //     });
            // });
            //bus.Start();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            app.UseAuthentication();

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
