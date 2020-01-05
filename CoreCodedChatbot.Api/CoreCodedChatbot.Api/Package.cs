using CoreCodedChatbot.Api.Commands;
using CoreCodedChatbot.Api.Interfaces.Commands;
using CoreCodedChatbot.Api.Interfaces.Queries;
using CoreCodedChatbot.Api.Interfaces.Services;
using CoreCodedChatbot.Api.Queries;
using CoreCodedChatbot.Api.Services;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Secrets;
using Microsoft.Extensions.DependencyInjection;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace CoreCodedChatbot.Api
{
    public static class Package
    {
        public static IServiceCollection AddTwitchServices(this IServiceCollection services, IConfigService configService, ISecretService secretService)
        {
            var api = new TwitchAPI();
            api.Settings.AccessToken = secretService.GetSecret<string>("ChatbotAccessToken");

            // TODO: Remove the need for the playlist service to talk directly in chat when opening the playlist.
            var creds = new ConnectionCredentials(configService.Get<string>("ChatbotNick"), secretService.GetSecret<string>("ChatbotPass"));
            var client = new TwitchClient();
            client.Initialize(creds, configService.Get<string>("StreamerChannel"));
            client.Connect();

            services.AddSingleton(api);
            services.AddSingleton(client);

            return services;
        }

        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddSingleton<IAzureDevOpsService, AzureDevOpsService>();

            return services;
        }

        public static IServiceCollection AddApiQueries(this IServiceCollection services)
        {
            services.AddTransient<IGetAllCurrentWorkItemsQuery, GetAllCurrentWorkItemsQuery>();
            services.AddTransient<IGetWorkItemByIdQuery, GetWorkItemByIdQuery>();

            return services;
        }

        public static IServiceCollection AddApiCommands(this IServiceCollection services)
        {
            services.AddTransient<IMapWorkItemsAndChildTasksToApiResponseModelsCommand, MapWorkItemsAndChildTasksToApiResponseModelsCommand>();
            services.AddTransient<IMapWorkItemToParentWorkItemCommand, MapWorkItemToParentWorkItemCommand>();
            services.AddTransient<IMapWorkItemToTaskCommand, MapWorkItemToTaskCommand>();

            return services;
        }

        public static IServiceCollection AddApiRepositories(this IServiceCollection services)
        {

            return services;
        }
    }
}