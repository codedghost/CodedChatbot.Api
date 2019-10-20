using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodedChatbot.Database.Context;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Library;
using CoreCodedChatbot.Library.Interfaces.Services;
using CoreCodedChatbot.Library.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace CoreCodedChatbot.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var config = new ConfigService().GetConfig();

            services.AddOptions();
            services.AddMemoryCache();

            services.AddAuthentication(op =>
                {
                    op.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    op.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    op.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.ApiSecretSymmetricKey)),
                        ValidateIssuer = true,
                        ValidIssuer = config.ApiValidIssuer,
                        ValidateAudience = true,
                        ValidAudience = config.ApiValidAudience
                    };
                });

            services
                .AddTwitchServices(config)
                .AddLibraryServices();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
