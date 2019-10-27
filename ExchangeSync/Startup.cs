using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ExchangeSync.Model;
using ExchangeSync.Model.Consumers;
using ExchangeSync.Services;
using ExchangeSync.Skype;
using MassTransit;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddHttpClient<IIdentityService, IdentityService>();
            services.AddHttpContextAccessor();
            var databseSection = Configuration.GetSection("Database");
            var mdmBusSection = Configuration.GetSection("MdmBus");
            var idSvrSection = Configuration.GetSection("IdSvr");
            services.Configure<IdSvrOption>(idSvrSection);
            var idSvrOption = services.BuildServiceProvider().GetService<IOptions<IdSvrOption>>();
            services.AddDbContext<ServiceDbContext>(option =>
            {
                option.UseSqlServer(databseSection.GetValue<string>("ConnectString"));
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            //services
            //    .AddAuthentication(options =>
            //    {
            //        options.DefaultScheme = "Cookies";
            //        options.DefaultChallengeScheme = "oidc";
            //    })
            //    .AddCookie("Cookies")
            //    .AddOpenIdConnect("oidc", options =>
            //    {
            //        options.SignInScheme = "Cookies";

            //        options.Authority = idSvrSection.GetValue<string>("IssuerUri");
            //        options.RequireHttpsMetadata = options.Authority.Contains("https");

            //        options.ClientId = idSvrSection.GetValue<string>("ClientId");
            //        options.ClientSecret = idSvrSection.GetValue<string>("ClientSecret");
            //        options.ResponseType = "code id_token";

            //        options.SaveTokens = true;
            //        options.GetClaimsFromUserInfoEndpoint = true;


            //        options.Events = new OpenIdConnectEvents()
            //        {
            //            OnTicketReceived = (context) =>
            //            {
            //                try
            //                {
            //                    var access_token = context.Properties.Items[".Token.access_token"];
            //                    if (!string.IsNullOrEmpty(access_token))
            //                    {
            //                        ClaimsIdentity claimsId = context.Principal.Identity as ClaimsIdentity;
            //                        if (claimsId != null)
            //                            claimsId.AddClaim(new Claim("access_token", access_token));
            //                        context.HttpContext.Response.Cookies.Append("access_token", access_token);
            //                    }
            //                }
            //                catch (Exception e)
            //                {
            //                    context.Response.Redirect("/home/NoAccess");
            //                }
            //                return Task.FromResult(0);
            //            },
            //            OnUserInformationReceived = (context) =>
            //            {
            //                try
            //                {
            //                    var settings = context.Request;
            //                    ClaimsIdentity claimsId = context.Principal.Identity as ClaimsIdentity;
            //                    //此处用于接收返回的用户信息基于GetClaimsFromUserInfoEndpoint = true;下面代码可以取消注释，
            //                    //查看已返回的用户信息并替换自己的代码逻辑实现。
            //                    if (claimsId != null)
            //                    {
            //                        var idcard_number = context.User.ContainsKey("idcard_number")
            //                            ? context.User["idcard_number"].ToString()
            //                            : null;
            //                        var role = context.User.ContainsKey("role")
            //                            ? context.User["role"].ToString()
            //                            : null;
            //                        var sub = context.User.ContainsKey("sub")
            //                            ? context.User["sub"].ToString()
            //                            : null;
            //                        var name = context.User.ContainsKey("name")
            //                            ? context.User["name"].ToString()
            //                            : null;
            //                        var preferred_username = context.User.ContainsKey("preferred_username")
            //                            ? context.User["preferred_username"].ToString()
            //                            : null;
            //                        if (idcard_number != null) claimsId.AddClaim(new Claim("idcard_number", idcard_number));
            //                        if (role != null) claimsId.AddClaim(new Claim("role", role));
            //                        if (sub != null) claimsId.AddClaim(new Claim("sub", sub));
            //                        if (name != null) claimsId.AddClaim(new Claim("name", name));
            //                        if (preferred_username != null) claimsId.AddClaim(new Claim("preferred_username", preferred_username));
            //                    }
            //                }
            //                catch (Exception e)
            //                {
            //                    context.Response.Redirect("/home/NoAccess");
            //                }

            //                return Task.FromResult(0);
            //            },
            //            //此处用于桌面登录程序使用，如果不需要集成到桌面登录程序里可以把此处代码移除
            //            #region
            //            //OnRedirectToIdentityProvider = (context) =>
            //            //{
            //            //    if (context.Request.Query.Any())
            //            //    {
            //            //        context.ProtocolMessage.Username = context.Request.Query.Keys.First();
            //            //    }
            //            //    return Task.FromResult(0);
            //            //}
            //            #endregion

            //        };
            //        foreach (string scope in idSvrOption.Value.Scopes)
            //        {
            //            options.Scope.Add(scope);
            //        }

            //        #region  如果出现丢失claims情况可以使用此小节代码试试
            //        //并参考一些链接https://stackoverflow.com/questions/46038509/unable-to-retrieve-claims-in-net-core-2-0
            //        //https://leastprivilege.com/2017/11/15/missing-claims-in-the-asp-net-core-2-openid-connect-handler/
            //        //options.Scope.Clear();
            //        //options.Scope.Add("openid");
            //        //options.Scope.Add("profile");
            //        //options.Scope.Add("email");
            //        //options.Scope.Add("api1");
            //        //options.Scope.Add("offline_access");

            //        //options.ClaimActions.Remove("amr");
            //        //options.ClaimActions.MapJsonKey("website", "website");
            //        #endregion

            //        //https://stackoverflow.com/a/38047051/377727
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuerSigningKey = false
            //        };
            //    });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IServerRenderService, ServerRenderService>(u => new ServerRenderService("./server.js"));
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ICalendarService, CalendarService>();
            services.AddScoped<IMeetingService, MeetingService>();
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
            //var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            // {
            //     var host = cfg.Host(new Uri(mdmBusSection.GetValue<string>("Host")), h =>
            //     {
            //         h.Username(mdmBusSection.GetValue<string>("UserName"));
            //         h.Password(mdmBusSection.GetValue<string>("Password"));
            //     });
            //     cfg.ReceiveEndpoint(host, "Local_Test_1", c =>
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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

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
