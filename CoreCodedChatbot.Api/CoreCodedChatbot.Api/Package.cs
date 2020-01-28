using CoreCodedChatbot.Api.Commands.AzureDevOps;
using CoreCodedChatbot.Api.Commands.GuessingGame;
using CoreCodedChatbot.Api.Commands.StreamStatus;
using CoreCodedChatbot.Api.Commands.Vip;
using CoreCodedChatbot.Api.Interfaces.Commands.AzureDevOps;
using CoreCodedChatbot.Api.Interfaces.Commands.GuessingGame;
using CoreCodedChatbot.Api.Interfaces.Commands.StreamStatus;
using CoreCodedChatbot.Api.Interfaces.Commands.Vip;
using CoreCodedChatbot.Api.Interfaces.Queries.AzureDevOps;
using CoreCodedChatbot.Api.Interfaces.Queries.GuessingGame;
using CoreCodedChatbot.Api.Interfaces.Queries.StreamStatus;
using CoreCodedChatbot.Api.Interfaces.Repositories.Bytes;
using CoreCodedChatbot.Api.Interfaces.Repositories.GuessingGame;
using CoreCodedChatbot.Api.Interfaces.Repositories.Settings;
using CoreCodedChatbot.Api.Interfaces.Repositories.StreamStatus;
using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Api.Interfaces.Services;
using CoreCodedChatbot.Api.Queries.AzureDevOps;
using CoreCodedChatbot.Api.Queries.GuessingGame;
using CoreCodedChatbot.Api.Queries.StreamStatus;
using CoreCodedChatbot.Api.Repositories.Bytes;
using CoreCodedChatbot.Api.Repositories.GuessingGame;
using CoreCodedChatbot.Api.Repositories.Settings;
using CoreCodedChatbot.Api.Repositories.StreamStatus;
using CoreCodedChatbot.Api.Repositories.Vip;
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
            services.AddSingleton<ISignalRService, SignalRService>();
            services.AddSingleton<IGuessingGameService, GuessingGameService>();
            services.AddSingleton<IVipService, VipService>();

            return services;
        }

        public static IServiceCollection AddApiQueries(this IServiceCollection services)
        {
            // DevOps
            services.AddSingleton<IGetAllCurrentWorkItemsQuery, GetAllCurrentWorkItemsQuery>();
            services.AddSingleton<IGetAllBacklogWorkItemsQuery, GetAllBacklogWorkItemsQuery>();
            services.AddSingleton<IGetWorkItemByIdQuery, GetWorkItemByIdQuery>();
            services.AddSingleton<IRaiseBugQuery, RaiseBugQuery>();
            services.AddSingleton<IGetDevOpsWorkItemIdsFromQueryId, GetDevOpsWorkItemIdsFromQueryId>();

            // Guessing Game
            services.AddSingleton<IGetCurrentGuessingGameMetadataQuery, GetCurrentGuessingGameMetadataQuery>();
            services.AddSingleton<IGetPotentialWinnersQuery, GetPotentialWinnersQuery>();

            // Stream Status
            services.AddSingleton<IGetStreamStatusQuery, GetStreamStatusQuery>();

            return services;
        }

        public static IServiceCollection AddApiCommands(this IServiceCollection services)
        {
            // DevOps
            services.AddSingleton<ICreateJsonPatchDocumentFromBugRequestCommand, CreateJsonPatchDocumentFromBugRequestCommand>();
            services.AddSingleton<IMapWorkItemsAndChildTasksToApiResponseModelsCommand, MapWorkItemsAndChildTasksToApiResponseModelsCommand>();
            services.AddSingleton<IMapWorkItemToParentWorkItemCommand, MapWorkItemToParentWorkItemCommand>();
            services.AddSingleton<IMapWorkItemToTaskCommand, MapWorkItemToTaskCommand>();

            // Guessing Game
            services.AddSingleton<ICompleteGuessingGameCommand, CompleteGuessingGameCommand>();
            services.AddSingleton<IGiveGuessingGameWinnersBytesCommand, GiveGuessingGameWinnersBytesCommand>();
            services.AddSingleton<ISetGuessingGameStateCommand, SetGuessingGameStateCommand>();
            services.AddSingleton<ISubmitOrUpdateGuessCommand, SubmitOrUpdateGuessCommand>();

            // Stream Status
            services.AddSingleton<ISaveStreamStatusCommand, SaveStreamStatusCommand>();

            // Vip
            services.AddSingleton<IRefundVipCommand, RefundVipCommand>();

            return services;
        }

        public static IServiceCollection AddApiRepositories(this IServiceCollection services)
        {
            // Bytes
            services.AddSingleton<IGiveUsersBytesRepository, GiveUsersBytesRepository>();

            // Guessing Game
            services.AddSingleton<ICloseGuessingGameRepository, CloseGuessingGameRepository>();
            services.AddSingleton<ICompleteGuessingGameRepository, CompleteGuessingGameRepository>();
            services.AddSingleton<IGetCurrentGuessingGameRepository, GetCurrentGuessingGameRepository>();
            services.AddSingleton<IGetGuessingGameStateQuery, GetGuessingGameStateQuery>();
            services.AddSingleton<IGetRunningGuessingGameIdRepository, GetRunningGuessingGameIdRepository>();
            services.AddSingleton<IGetSongPercentageGuessesRepository, GetSongPercentageGuessesRepository>();
            services.AddSingleton<IOpenGuessingGameRepository, OpenGuessingGameRepository>();
            services.AddSingleton<ISubmitOrUpdateGuessRepository, SubmitOrUpdateGuessRepository>();

            // Settings
            services.AddSingleton<IGetSettingRepository, GetSettingRepository>();
            services.AddSingleton<ISetOrCreateSettingRepository, SetOrCreateSettingRepository>();
            
            // Stream Status
            services.AddSingleton<IGetStreamStatusRepository, GetStreamStatusRepository>();
            services.AddSingleton<ISaveStreamStatusRepository, SaveStreamStatusRepository>();

            // Vip
            services.AddSingleton<IRefundVipsRepository, RefundVipsRepository>();
            services.AddSingleton<IGiftVipRepository, GiftVipRepository>();

            return services;
        }
    }
}