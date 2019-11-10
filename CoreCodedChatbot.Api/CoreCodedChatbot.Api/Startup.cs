using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Database;
using CoreCodedChatbot.Database.Context;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Library;
using CoreCodedChatbot.Library.Interfaces.Services;
using CoreCodedChatbot.Library.Services;
using CoreCodedChatbot.Secrets;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            var configService = new ConfigService();

            services.AddOptions();
            services.AddMemoryCache();

            services
                .AddChatbotConfigService()
                .AddChatbotSecretServiceCollection(
                    configService.Get<string>("KeyVaultAppId"),
                    configService.Get<string>("KeyVaultCertThumbprint"),
                    configService.Get<string>("KeyVaultBaseUrl")
                );

            var secretService = services.BuildServiceProvider().GetService<ISecretService>();

            services.AddAuthentication(op =>
                {
                    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    op.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretService.GetSecret<string>("ApiSecretSymmetricKey"))),
                        ValidateIssuer = true,
                        ValidIssuer = secretService.GetSecret<string>("ApiValidIssuer"),
                        ValidateAudience = true,
                        ValidAudience = secretService.GetSecret<string>("ApiValidAudience")
                    };
                });

            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme);
                defaultAuthorizationPolicyBuilder =
                    defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });

            services
                .AddDbContextFactory()
                .AddTwitchServices(configService, secretService)
                .AddLibraryServices();

            services.AddRouting();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            using (var context = (ChatbotContext)serviceProvider.GetService<IChatbotContextFactory>().Create())
            {
                context.Database.Migrate();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
