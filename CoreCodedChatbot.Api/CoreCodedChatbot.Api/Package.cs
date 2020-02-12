using CoreCodedChatbot.Api.Commands.AzureDevOps;
using CoreCodedChatbot.Api.Commands.GuessingGame;
using CoreCodedChatbot.Api.Commands.Playlist;
using CoreCodedChatbot.Api.Commands.Quote;
using CoreCodedChatbot.Api.Commands.StreamStatus;
using CoreCodedChatbot.Api.Commands.Vip;
using CoreCodedChatbot.Api.Interfaces.Commands.AzureDevOps;
using CoreCodedChatbot.Api.Interfaces.Commands.GuessingGame;
using CoreCodedChatbot.Api.Interfaces.Commands.Playlist;
using CoreCodedChatbot.Api.Interfaces.Commands.Quote;
using CoreCodedChatbot.Api.Interfaces.Commands.StreamStatus;
using CoreCodedChatbot.Api.Interfaces.Commands.Vip;
using CoreCodedChatbot.Api.Interfaces.Queries.AzureDevOps;
using CoreCodedChatbot.Api.Interfaces.Queries.GuessingGame;
using CoreCodedChatbot.Api.Interfaces.Queries.Playlist;
using CoreCodedChatbot.Api.Interfaces.Queries.Quote;
using CoreCodedChatbot.Api.Interfaces.Queries.StreamStatus;
using CoreCodedChatbot.Api.Interfaces.Queries.Vip;
using CoreCodedChatbot.Api.Interfaces.Repositories.Bytes;
using CoreCodedChatbot.Api.Interfaces.Repositories.GuessingGame;
using CoreCodedChatbot.Api.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.Api.Interfaces.Repositories.Quote;
using CoreCodedChatbot.Api.Interfaces.Repositories.Settings;
using CoreCodedChatbot.Api.Interfaces.Repositories.StreamStatus;
using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Api.Interfaces.Services;
using CoreCodedChatbot.Api.Queries.AzureDevOps;
using CoreCodedChatbot.Api.Queries.GuessingGame;
using CoreCodedChatbot.Api.Queries.Playlist;
using CoreCodedChatbot.Api.Queries.Quote;
using CoreCodedChatbot.Api.Queries.StreamStatus;
using CoreCodedChatbot.Api.Queries.Vip;
using CoreCodedChatbot.Api.Repositories.Bytes;
using CoreCodedChatbot.Api.Repositories.GuessingGame;
using CoreCodedChatbot.Api.Repositories.Playlist;
using CoreCodedChatbot.Api.Repositories.Quote;
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
using IModGiveVipRepository = CoreCodedChatbot.Api.Interfaces.Repositories.Vip.IModGiveVipRepository;

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

            services.AddSingleton<IQuoteService, QuoteService>();

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

            // Playlist
            services.AddSingleton<ICheckUserHasMaxRegularsInQueueQuery, CheckUserHasMaxRegularsInQueueQuery>();
            services.AddSingleton<IGetPlaylistStateQuery, GetPlaylistStateQuery>();
            services.AddSingleton<IGetSongRequestByIdQuery, GetSongRequestByIdQuery>();
            services.AddSingleton<IGetUsersCurrentRequestCountsQuery, GetUsersCurrentRequestCountsQuery>();
            services.AddSingleton<IIsSuperVipInQueueQuery, IsSuperVipInQueueQuery>();
            services.AddSingleton<IGetCurrentRequestsQuery, GetCurrentRequestsQuery>();
            services.AddSingleton<IGetUsersFormattedRequestsQuery, GetUsersFormattedRequestsQuery>();

            // Quote
            services.AddSingleton<IGetRandomQuoteQuery, GetRandomQuoteQuery>();
            services.AddSingleton<IGetQuoteQuery, GetQuoteQuery>();

            // Stream Status
            services.AddSingleton<IGetStreamStatusQuery, GetStreamStatusQuery>();

            // Vip
            services.AddSingleton<ICheckUserHasVipsQuery, CheckUserHasVipsQuery>();
            services.AddSingleton<IGetUsersGiftedVipsQuery, GetUsersGiftedVipsQuery>();

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

            // Playlist
            services.AddSingleton<IAddSongRequestCommand, AddSongRequestCommand>();
            services.AddSingleton<IPromoteUsersRegularRequestCommand, PromoteUsersRegularRequestCommand>();
            services.AddSingleton<IArchiveRequestCommand, ArchiveRequestCommand>();
            services.AddSingleton<IRemoveAndRefundAllRequestsCommand, RemoveAndRefundAllRequestsCommand>();
            services.AddSingleton<IUpdatePlaylistStateCommand, UpdatePlaylistStateCommand>();
            services.AddSingleton<IAddSongToDriveCommand, AddSongToDriveCommand>();

            // Quote
            services.AddSingleton<IAddQuoteCommand, AddQuoteCommand>();
            services.AddSingleton<IEditQuoteCommand, EditQuoteCommand>();
            services.AddSingleton<IRemoveQuoteCommand, RemoveQuoteCommand>();

            // Stream Status
            services.AddSingleton<ISaveStreamStatusCommand, SaveStreamStatusCommand>();

            // Vip
            services.AddSingleton<IRefundVipCommand, RefundVipCommand>();
            services.AddSingleton<IGiftVipCommand, GiftVipCommand>();
            services.AddSingleton<IUseVipCommand, UseVipCommand>();
            services.AddSingleton<IUseSuperVipCommand, UseSuperVipCommand>();
            services.AddSingleton<IModGiveVipCommand, ModGiveVipCommand>();

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

            // Playlist
            services.AddSingleton<IAddRequestRepository, AddRequestRepository>();
            services.AddSingleton<IGetIsUserInChatRepository, GetIsUserInChatRepository>();
            services.AddSingleton<IGetSongRequestByIdRepository, GetSongRequestByIdRepository>();
            services.AddSingleton<IGetUsersBytesCountRepository, GetUsersBytesCountRepository>();
            services.AddSingleton<IGetUsersCurrentRegularRequestCountRepository, GetUsersCurrentRegularRequestCountRepository>();
            services.AddSingleton<IGetUsersCurrentRequestCountRepository, GetUsersCurrentRequestCountRepository>();
            services.AddSingleton<IPromoteUsersRegularRequestCommand, PromoteUsersRegularRequestCommand>();
            services.AddSingleton<IArchiveRequestRepository, ArchiveRequestRepository>();
            services.AddSingleton<IClearRequestsRepository, ClearRequestsRepository>();
            services.AddSingleton<IGetCurrentRequestsRepository, GetCurrentRequestsRepository>();
            services.AddSingleton<IGetUsersRequestsRepository, GetUsersRequestsRepository>();
            services.AddSingleton<IAddSongToDriveRepository, AddSongToDriveRepository>();

            // Quote
            services.AddSingleton<IAddQuoteRepository, AddQuoteRepository>();
            services.AddSingleton<IEditQuoteRepository, EditQuoteRepository>();
            services.AddSingleton<IRemoveQuoteRepository, RemoveQuoteRepository>();
            services.AddSingleton<IGetQuoteRepository, GetQuoteRepository>();
            services.AddSingleton<IGetQuoteIdsRepository, GetQuoteIdsRepository>();

            // Settings
            services.AddSingleton<IGetSettingRepository, GetSettingRepository>();
            services.AddSingleton<ISetOrCreateSettingRepository, SetOrCreateSettingRepository>();

            // Stream Status
            services.AddSingleton<IGetStreamStatusRepository, GetStreamStatusRepository>();
            services.AddSingleton<ISaveStreamStatusRepository, SaveStreamStatusRepository>();

            // Vip
            services.AddSingleton<IGetUsersCurrentSuperVipRequestCountRepository, GetUsersCurrentSuperVipRequestCountRepository>();
            services.AddSingleton<IGetUsersCurrentVipRequestCountRepository, GetUsersCurrentVipRequestCountRepository>();
            services.AddSingleton<IGetUsersVipCountRepository, GetUsersVipCountRepository>();
            services.AddSingleton<IGetUsersGiftedVipsRepository, GetUsersGiftedVipsRepository>();
            services.AddSingleton<IRefundVipsRepository, RefundVipsRepository>();
            services.AddSingleton<IGiftVipRepository, GiftVipRepository>();
            services.AddSingleton<IUseVipRepository, UseVipRepository>();
            services.AddSingleton<IUseSuperVipRepository, UseSuperVipRepository>();
            services.AddSingleton<IModGiveVipRepository, ModGiveVipRepository>();
            services.AddSingleton<IIsSuperVipInQueueRepository, IsSuperVipInQueueRepository>();

            return services;
        }
    }
}