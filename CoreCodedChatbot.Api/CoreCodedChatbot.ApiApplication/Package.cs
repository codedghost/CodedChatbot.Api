using CoreCodedChatbot.ApiApplication.Commands.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Commands.GuessingGame;
using CoreCodedChatbot.ApiApplication.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Commands.Quote;
using CoreCodedChatbot.ApiApplication.Commands.StreamStatus;
using CoreCodedChatbot.ApiApplication.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.StreamStatus;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.StreamStatus;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Settings;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.StreamStatus;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Queries.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Queries.GuessingGame;
using CoreCodedChatbot.ApiApplication.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Queries.Quote;
using CoreCodedChatbot.ApiApplication.Queries.StreamStatus;
using CoreCodedChatbot.ApiApplication.Queries.Vip;
using CoreCodedChatbot.ApiApplication.Repositories.Bytes;
using CoreCodedChatbot.ApiApplication.Repositories.GuessingGame;
using CoreCodedChatbot.ApiApplication.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Repositories.Quote;
using CoreCodedChatbot.ApiApplication.Repositories.Settings;
using CoreCodedChatbot.ApiApplication.Repositories.StreamStatus;
using CoreCodedChatbot.ApiApplication.Repositories.Vip;
using CoreCodedChatbot.ApiApplication.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CoreCodedChatbot.ApiApplication
{
    public static class Package
    {
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
            services.AddSingleton<IGetMaxRegularRequestCountQuery, GetMaxRegularRequestCountQuery>();

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
            services.AddSingleton<IEditSuperVipCommand, EditSuperVipCommand>();
            services.AddSingleton<IProcessRegularSongRequestCommand, ProcessRegularSongRequestCommand>();
            services.AddSingleton<IProcessVipSongRequestCommand, ProcessVipSongRequestCommand>();
            services.AddSingleton<IProcessSuperVipSongRequestCommand, ProcessSuperVipSongRequestCommand>();
            services.AddSingleton<IProcessSongRequestCommand, ProcessSongRequestCommand>();

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
            services.AddSingleton<IEditSuperVipRequestRepository, EditSuperVipRequestRepository>();

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
