using Core.Signalr.Template.Client.Helpers;
using Core.Signalr.Template.Client.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Core.Signalr.Template.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment webEnv)
        {
            Configuration = configuration;
            _webEnv = webEnv;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _webEnv;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSection = Configuration.GetSection("JwtSetting");
            services.Configure<JwtSetting>(option => appSection.Bind(option));
            var appSetting = appSection.Get<JwtSetting>();

            // services.AddHostedService<ClearBackGroundService>();

            services.AddSingleton<SignalrHelper>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, cookieOption =>
                 {
                     cookieOption.LoginPath = "/Account/Login";
                     cookieOption.AccessDeniedPath = "/Account/Login";
                 });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc(options => options.Filters.Add(new AuthorizeFilter()))
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (_webEnv.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();
                // app.UseHttpsRedirection();
            }

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "api",
                    pattern: "api/{controller=Values}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
