﻿using CoreCodedChatbot.ApiApplication.Commands.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Commands.GuessingGame;
using CoreCodedChatbot.ApiApplication.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Commands.Quote;
using CoreCodedChatbot.ApiApplication.Commands.StreamStatus;
using CoreCodedChatbot.ApiApplication.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Search;
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
            services.AddSingleton<IGuessingGameService, GuessingGameService>();
            services.AddSingleton<IPlaylistService, PlaylistService>();
            services.AddSingleton<IQuoteService, QuoteService>();
            services.AddSingleton<ISearchService, SearchService>();
            services.AddSingleton<ISignalRService, SignalRService>();
            services.AddSingleton<IVipService, VipService>();

            return services;
        }

        public static IServiceCollection AddApiQueries(this IServiceCollection services)
        {
            // DevOps
            services.AddSingleton<IGetAllBacklogWorkItemsQuery, GetAllBacklogWorkItemsQuery>();
            services.AddSingleton<IGetAllCurrentWorkItemsQuery, GetAllCurrentWorkItemsQuery>();
            services.AddSingleton<IGetDevOpsWorkItemIdsFromQueryId, GetDevOpsWorkItemIdsFromQueryId>();
            services.AddSingleton<IGetWorkItemByIdQuery, GetWorkItemByIdQuery>();
            services.AddSingleton<IRaiseBugQuery, RaiseBugQuery>();

            // Guessing Game
            services.AddSingleton<IGetCurrentGuessingGameMetadataQuery, GetCurrentGuessingGameMetadataQuery>();
            services.AddSingleton<IGetPotentialWinnersQuery, GetPotentialWinnersQuery>();

            // Playlist
            services.AddSingleton<ICheckUserHasMaxRegularsInQueueQuery, CheckUserHasMaxRegularsInQueueQuery>();
            services.AddSingleton<IGetCurrentRequestsQuery, GetCurrentRequestsQuery>();
            services.AddSingleton<IGetMaxRegularRequestCountQuery, GetMaxRegularRequestCountQuery>();
            services.AddSingleton<IGetPlaylistStateQuery, GetPlaylistStateQuery>();
            services.AddSingleton<IGetSingleSongRequestIdQuery, GetSingleSongRequestIdQuery>();
            services.AddSingleton<IGetSongRequestByIdQuery, GetSongRequestByIdQuery>();
            services.AddSingleton<IGetUsersCurrentRequestCountsQuery, GetUsersCurrentRequestCountsQuery>();
            services.AddSingleton<IGetUsersFormattedRequestsQuery, GetUsersFormattedRequestsQuery>();
            services.AddSingleton<IGetUsersRequestAtPlaylistIndexQuery, GetUsersRequestAtPlaylistIndexQuery>();
            services.AddSingleton<IIsSuperVipInQueueQuery, IsSuperVipInQueueQuery>();

            // Quote
            services.AddSingleton<IGetQuoteQuery, GetQuoteQuery>();
            services.AddSingleton<IGetRandomQuoteQuery, GetRandomQuoteQuery>();

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
            services.AddSingleton<ICreateJsonPatchDocumentFromProductBacklogItemRequestCommand, CreateJsonPatchDocumentFromProductBacklogItemRequestCommand>();
            services.AddSingleton<ICreateJsonPatchForWorkItemCommand, CreateJsonPatchForWorkItemCommand>();
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
            services.AddSingleton<IAddSongToDriveCommand, AddSongToDriveCommand>();
            services.AddSingleton<IArchiveRequestCommand, ArchiveRequestCommand>();
            services.AddSingleton<IArchiveUsersSingleRequestCommand, ArchiveUsersSingleRequestCommand>();
            services.AddSingleton<IEditRequestCommand, EditRequestCommand>();
            services.AddSingleton<IEditSuperVipCommand, EditSuperVipCommand>();
            services.AddSingleton<IProcessRegularSongRequestCommand, ProcessRegularSongRequestCommand>();
            services.AddSingleton<IProcessSongRequestCommand, ProcessSongRequestCommand>();
            services.AddSingleton<IProcessSuperVipSongRequestCommand, ProcessSuperVipSongRequestCommand>();
            services.AddSingleton<IProcessVipSongRequestCommand, ProcessVipSongRequestCommand>();
            services.AddSingleton<IPromoteUsersRegularRequestCommand, PromoteUsersRegularRequestCommand>();
            services.AddSingleton<IRemoveAndRefundAllRequestsCommand, RemoveAndRefundAllRequestsCommand>();
            services.AddSingleton<IRemoveSuperVipCommand, RemoveSuperVipCommand>();
            services.AddSingleton<IRemoveUsersRequestByPlaylistIndexCommand, RemoveUsersRequestByPlaylistIndexCommand>();
            services.AddSingleton<IUpdatePlaylistStateCommand, UpdatePlaylistStateCommand>();

            // Quote
            services.AddSingleton<IAddQuoteCommand, AddQuoteCommand>();
            services.AddSingleton<IEditQuoteCommand, EditQuoteCommand>();
            services.AddSingleton<IRemoveQuoteCommand, RemoveQuoteCommand>();

            // Search
            services.AddSingleton<ISaveSearchSynonymRequestCommand, SaveSearchSynonymRequestCommand>();

            // Stream Status
            services.AddSingleton<ISaveStreamStatusCommand, SaveStreamStatusCommand>();

            // Vip
            services.AddSingleton<IGiftVipCommand, GiftVipCommand>();
            services.AddSingleton<IModGiveVipCommand, ModGiveVipCommand>();
            services.AddSingleton<IRefundVipCommand, RefundVipCommand>();
            services.AddSingleton<IUseSuperVipCommand, UseSuperVipCommand>();
            services.AddSingleton<IUseVipCommand, UseVipCommand>();

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
            services.AddSingleton<IAddSongToDriveRepository, AddSongToDriveRepository>();
            services.AddSingleton<IArchiveRequestRepository, ArchiveRequestRepository>();
            services.AddSingleton<IClearRequestsRepository, ClearRequestsRepository>();
            services.AddSingleton<IEditRequestRepository, EditRequestRepository>();
            services.AddSingleton<IEditSuperVipRequestRepository, EditSuperVipRequestRepository>();
            services.AddSingleton<IGetCurrentRequestsRepository, GetCurrentRequestsRepository>();
            services.AddSingleton<IGetIsUserInChatRepository, GetIsUserInChatRepository>();
            services.AddSingleton<IGetSingleSongRequestIdRepository, GetSingleSongRequestIdRepository>();
            services.AddSingleton<IGetSongRequestByIdRepository, GetSongRequestByIdRepository>();
            services.AddSingleton<IGetUsersBytesCountRepository, GetUsersBytesCountRepository>();
            services.AddSingleton<IGetUsersCurrentRegularRequestCountRepository, GetUsersCurrentRegularRequestCountRepository>();
            services.AddSingleton<IGetUsersCurrentRequestCountRepository, GetUsersCurrentRequestCountRepository>();
            services.AddSingleton<IGetUsersRequestsRepository, GetUsersRequestsRepository>();
            services.AddSingleton<IPromoteUserRequestRepository, PromoteUserRequestRepository>();
            services.AddSingleton<IRemoveRegularRequestRepository, RemoveRegularRequestRepository>();
            services.AddSingleton<IRemoveSuperVipRepository, RemoveSuperVipRepository>();

            // Quote
            services.AddSingleton<IAddQuoteRepository, AddQuoteRepository>();
            services.AddSingleton<IEditQuoteRepository, EditQuoteRepository>();
            services.AddSingleton<IGetQuoteIdsRepository, GetQuoteIdsRepository>();
            services.AddSingleton<IGetQuoteRepository, GetQuoteRepository>();
            services.AddSingleton<IRemoveQuoteRepository, RemoveQuoteRepository>();

            // Search
            services.AddSingleton<ISaveSearchSynonymRequestRepository, SaveSearchSynonymRequestRepository>();

            // Settings
            services.AddSingleton<IGetSettingRepository, GetSettingRepository>();
            services.AddSingleton<ISetOrCreateSettingRepository, SetOrCreateSettingRepository>();

            // Stream Status
            services.AddSingleton<IGetStreamStatusRepository, GetStreamStatusRepository>();
            services.AddSingleton<ISaveStreamStatusRepository, SaveStreamStatusRepository>();

            // Vip
            services.AddSingleton<IGetUsersCurrentSuperVipRequestCountRepository, GetUsersCurrentSuperVipRequestCountRepository>();
            services.AddSingleton<IGetUsersCurrentVipRequestCountRepository, GetUsersCurrentVipRequestCountRepository>();
            services.AddSingleton<IGetUsersGiftedVipsRepository, GetUsersGiftedVipsRepository>();
            services.AddSingleton<IGetUsersVipCountRepository, GetUsersVipCountRepository>();
            services.AddSingleton<IGiftVipRepository, GiftVipRepository>();
            services.AddSingleton<IIsSuperVipInQueueRepository, IsSuperVipInQueueRepository>();
            services.AddSingleton<IModGiveVipRepository, ModGiveVipRepository>();
            services.AddSingleton<IRefundVipsRepository, RefundVipsRepository>();
            services.AddSingleton<IUseSuperVipRepository, UseSuperVipRepository>();
            services.AddSingleton<IUseVipRepository, UseVipRepository>();

            return services;
        }
    }
}
