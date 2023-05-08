using System;
using System.Text;
using System.Threading.Tasks;
using CodedGhost.Config;
using CoreCodedChatbot.ApiApplication;
using CoreCodedChatbot.ApiApplication.Factories;
using CoreCodedChatbot.ApiApplication.Hubs;
using CoreCodedChatbot.ApiApplication.Interfaces.Factories;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Services;
using CoreCodedChatbot.ApiContract.SignalRHubModels.API;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Database;
using CoreCodedChatbot.Database.Context;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Logging;
using CoreCodedChatbot.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace CoreCodedChatbot.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var configService = new ConfigService();

            services.AddOptions();
            services.AddMemoryCache();

            services.AddChatbotConfigService();

            var secretService = new AzureKeyVaultService(
                configService.Get<string>("KeyVaultAppId"),
                configService.Get<string>("KeyVaultCertThumbprint"),
                configService.Get<string>("KeyVaultBaseUrl"),
                configService.Get<string>("ActiveDirectoryTenantId"));

            secretService.Initialize().Wait();
            services.AddSingleton<ISecretService, AzureKeyVaultService>(provider => secretService);

            services
                .AddDbContextFactory()
                .AddChatbotNLog()
                .AddTwitchServices()
                .AddApplicationServices()
                .AddAutoMapper()
                .AddSolr(secretService)
                .AddRabbitConnectionServices()
                .AddPrintfulClient(secretService)
                .AddFactories();

            services.AddSignalR();
            services.AddRouting();

            services.AddAuthentication(op =>
                {
                    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    op.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                });

            services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
                .Configure<ISecretService>((options, secrets) =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secrets.GetSecret<string>("ApiSecretSymmetricKey"))),
                        ValidateIssuer = true,
                        ValidIssuer = secrets.GetSecret<string>("ApiValidIssuer"),
                        ValidateAudience = true,
                        ValidAudience = secrets.GetSecret<string>("ApiValidAudience")
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, builder =>
                {
                    builder.RequireAuthenticatedUser()
                        .Build();
                });
            });

            services.AddControllers().AddNewtonsoftJson(x =>
                x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env, IServiceProvider serviceProvider)
        {
            using (var context = (ChatbotContext)serviceProvider.GetService<IChatbotContextFactory>().Create())
            {
                context.Database.Migrate();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            if (env.IsDevelopment() || env.IsEnvironment("Local"))
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHub<BackgroundSongHub>(APIHubConstants.BackgroundSongHubPath);
            });

            var streamLabsService = (StreamLabsService) serviceProvider.GetService<IStreamLabsService>();
            var chatterService = (ChatService) serviceProvider.GetService<IChatService>();
            var channelRewardsService = (ChannelRewardsService) serviceProvider.GetService<IChannelRewardsService>();
            var printfulFactory =
                (PrintfulWebhookSetupFactory) serviceProvider.GetService<IPrintfulWebhookSetupFactory>();

            streamLabsService.Initialise();
            chatterService.Initialise();
            channelRewardsService.Initialise();

            try
            {
                var setupWebhookTask = printfulFactory.SetupPrintfulWebhook();
                Task.WaitAll(setupWebhookTask);
            }
            catch (Exception _)
            {
                Console.WriteLine("Failed to set up prinful webhook");
            }
            
        }
    }
}
