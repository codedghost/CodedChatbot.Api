using CoreCodedChatbot.Api.Commands;
using CoreCodedChatbot.Api.Interfaces.Commands;
using CoreCodedChatbot.Api.Interfaces.Queries;
using CoreCodedChatbot.Api.Interfaces.Repositories;
using CoreCodedChatbot.Api.Interfaces.Services;
using CoreCodedChatbot.Api.Queries;
using CoreCodedChatbot.Api.Repositories;
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

            services.AddSingleton<IQuoteService, QuoteService>();
            services.AddSingleton<ISearchService, SearchService>();

            return services;
        }

        public static IServiceCollection AddApiQueries(this IServiceCollection services)
        {
            services.AddSingleton<IGetAllCurrentWorkItemsQuery, GetAllCurrentWorkItemsQuery>();
            services.AddSingleton<IGetAllBacklogWorkItemsQuery, GetAllBacklogWorkItemsQuery>();
            services.AddSingleton<IGetWorkItemByIdQuery, GetWorkItemByIdQuery>();
            services.AddSingleton<IRaiseBugQuery, RaiseBugQuery>();
            services.AddSingleton<IGetDevOpsWorkItemIdsFromQueryId, GetDevOpsWorkItemIdsFromQueryId>();
            services.AddSingleton<IGetStreamStatusQuery, GetStreamStatusQuery>();

            services.AddSingleton<IGetRandomQuoteQuery, GetRandomQuoteQuery>();
            services.AddSingleton<IGetQuoteQuery, GetQuoteQuery>();

            return services;
        }

        public static IServiceCollection AddApiCommands(this IServiceCollection services)
        {
            services.AddSingleton<ICreateJsonPatchDocumentFromBugRequestCommand, CreateJsonPatchDocumentFromBugRequestCommand>();
            services.AddSingleton<ICreateJsonPatchDocumentFromProductBacklogItemRequestCommand, CreateJsonPatchDocumentFromProductBacklogItemRequestCommand>();
            services.AddSingleton<IMapWorkItemsAndChildTasksToApiResponseModelsCommand, MapWorkItemsAndChildTasksToApiResponseModelsCommand>();
            services.AddSingleton<IMapWorkItemToParentWorkItemCommand, MapWorkItemToParentWorkItemCommand>();
            services.AddSingleton<IMapWorkItemToTaskCommand, MapWorkItemToTaskCommand>();
            services.AddSingleton<ISaveStreamStatusCommand, SaveStreamStatusCommand>();

            services.AddSingleton<IAddQuoteCommand, AddQuoteCommand>();
            services.AddSingleton<IEditQuoteCommand, EditQuoteCommand>();
            services.AddSingleton<IRemoveQuoteCommand, RemoveQuoteCommand>();

            services.AddSingleton<ISaveSearchSynonymRequestCommand, SaveSearchSynonymRequestCommand>();

            return services;
        }

        public static IServiceCollection AddApiRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IGetStreamStatusRepository, GetStreamStatusRepository>();
            services.AddSingleton<ISaveStreamStatusRepository, SaveStreamStatusRepository>();

            services.AddSingleton<IAddQuoteRepository, AddQuoteRepository>();
            services.AddSingleton<IEditQuoteRepository, EditQuoteRepository>();
            services.AddSingleton<IRemoveQuoteRepository, RemoveQuoteRepository>();
            services.AddSingleton<IGetQuoteRepository, GetQuoteRepository>();
            services.AddSingleton<IGetQuoteIdsRepository, GetQuoteIdsRepository>();
            services.AddSingleton<ISaveSearchSynonymRequestRepository, SaveSearchSynonymRequestRepository>();

            return services;
        }
    }
}