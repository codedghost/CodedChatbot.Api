using System;
using System.Net.Http.Headers;
using System.Text;
using CoreCodedChatbot.ApiApplication.Commands.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Commands.Bytes;
using CoreCodedChatbot.ApiApplication.Commands.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Commands.ChatCommand;
using CoreCodedChatbot.ApiApplication.Commands.GuessingGame;
using CoreCodedChatbot.ApiApplication.Commands.Moderation;
using CoreCodedChatbot.ApiApplication.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Commands.Quote;
using CoreCodedChatbot.ApiApplication.Commands.StreamStatus;
using CoreCodedChatbot.ApiApplication.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Bytes;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.ChatCommand;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Moderation;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.StreamStatus;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Bytes;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.ChatCommand;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.StreamLabs;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.StreamStatus;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChatCommand;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Moderation;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Settings;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.StreamLabs;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.StreamStatus;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Solr;
using CoreCodedChatbot.ApiApplication.Queries.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Queries.Bytes;
using CoreCodedChatbot.ApiApplication.Queries.ChatCommand;
using CoreCodedChatbot.ApiApplication.Queries.GuessingGame;
using CoreCodedChatbot.ApiApplication.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Queries.Quote;
using CoreCodedChatbot.ApiApplication.Queries.Search;
using CoreCodedChatbot.ApiApplication.Queries.StreamLabs;
using CoreCodedChatbot.ApiApplication.Queries.StreamStatus;
using CoreCodedChatbot.ApiApplication.Queries.Vip;
using CoreCodedChatbot.ApiApplication.Repositories.Bytes;
using CoreCodedChatbot.ApiApplication.Repositories.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Repositories.ChatCommand;
using CoreCodedChatbot.ApiApplication.Repositories.GuessingGame;
using CoreCodedChatbot.ApiApplication.Repositories.Moderation;
using CoreCodedChatbot.ApiApplication.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Repositories.Quote;
using CoreCodedChatbot.ApiApplication.Repositories.Search;
using CoreCodedChatbot.ApiApplication.Repositories.Settings;
using CoreCodedChatbot.ApiApplication.Repositories.StreamLabs;
using CoreCodedChatbot.ApiApplication.Repositories.StreamStatus;
using CoreCodedChatbot.ApiApplication.Repositories.Vip;
using CoreCodedChatbot.ApiApplication.Services;
using CoreCodedChatbot.Secrets;
using Microsoft.Extensions.DependencyInjection;
using SolrNet;

namespace CoreCodedChatbot.ApiApplication
{
    public static class Package
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddSingleton<IAzureDevOpsService, AzureDevOpsService>();
            services.AddSingleton<IChannelRewardsService, ChannelRewardsService>();
            services.AddSingleton<IChatCommandService, ChatCommandService>();
            services.AddSingleton<IChatService, ChatService>();
            services.AddSingleton<IDownloadChartService, DownloadChartService>();
            services.AddSingleton<IGuessingGameService, GuessingGameService>();
            services.AddSingleton<IModerationService, ModerationService>();
            services.AddSingleton<IPlaylistService, PlaylistService>();
            services.AddSingleton<IQuoteService, QuoteService>();
            services.AddSingleton<ISearchService, SearchService>();
            services.AddSingleton<ISolrService, SolrService>();
            services.AddSingleton<ISignalRService, SignalRService>();
            services.AddSingleton<IStreamLabsService, StreamLabsService>();
            services.AddSingleton<IVipService, VipService>();

            return services;
        }

        public static IServiceCollection AddApiQueries(this IServiceCollection services)
        {
            // Azure DevOps
            services.AddSingleton<IGetAllBacklogWorkItemsQuery, GetAllBacklogWorkItemsQuery>();
            services.AddSingleton<IGetAllCurrentWorkItemsQuery, GetAllCurrentWorkItemsQuery>();
            services.AddSingleton<IGetDevOpsWorkItemIdsFromQueryId, GetDevOpsWorkItemIdsFromQueryId>();
            services.AddSingleton<IGetWorkItemByIdQuery, GetWorkItemByIdQuery>();
            services.AddSingleton<IRaiseBugQuery, RaiseBugQuery>();

            // Bytes
            services.AddSingleton<IGetUserByteCountQuery, GetUserByteCountQuery>();

            // Chat Command
            services.AddSingleton<IGetCommandHelpTextByKeywordQuery, GetCommandHelpTextByKeywordQuery>();
            services.AddSingleton<IGetCommandTextByKeywordQuery, GetCommandTextByKeywordQuery>();

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
            services.AddSingleton<IGetTopTenRequestsQuery, GetTopTenRequestsQuery>();
            services.AddSingleton<IGetUsersCurrentRequestCountsQuery, GetUsersCurrentRequestCountsQuery>();
            services.AddSingleton<IGetUsersFormattedRequestsQuery, GetUsersFormattedRequestsQuery>();
            services.AddSingleton<IGetUsersRequestAtPlaylistIndexQuery, GetUsersRequestAtPlaylistIndexQuery>();
            services.AddSingleton<IIsSuperVipInQueueQuery, IsSuperVipInQueueQuery>();

            // Quote
            services.AddSingleton<IGetQuoteQuery, GetQuoteQuery>();
            services.AddSingleton<IGetQuotesQuery, GetQuotesQuery>();
            services.AddSingleton<IGetRandomQuoteQuery, GetRandomQuoteQuery>();

            // Search
            services.AddSingleton<IGetSongBySearchIdQuery, GetSongBySearchIdQuery>();
            services.AddSingleton<IGetSongsFromSearchResultsQuery, GetSongsFromSearchResultsQuery>();

            // StreamLabs
            services.AddSingleton<IGetRecentDonationsQuery, GetRecentDonationsQuery>();

            // Stream Status
            services.AddSingleton<IGetStreamStatusQuery, GetStreamStatusQuery>();

            // Vip
            services.AddSingleton<ICheckUserHasVipsQuery, CheckUserHasVipsQuery>();
            services.AddSingleton<IGetUsersGiftedVipsQuery, GetUsersGiftedVipsQuery>();
            services.AddSingleton<IGetUserVipCountQuery, GetUserVipCountQuery>();

            return services;
        }

        public static IServiceCollection AddApiCommands(this IServiceCollection services)
        {
            // Azure DevOps
            services.AddSingleton<ICreateJsonPatchDocumentFromBugRequestCommand, CreateJsonPatchDocumentFromBugRequestCommand>();
            services.AddSingleton<ICreateJsonPatchDocumentFromProductBacklogItemRequestCommand, CreateJsonPatchDocumentFromProductBacklogItemRequestCommand>();
            services.AddSingleton<ICreateJsonPatchForWorkItemCommand, CreateJsonPatchForWorkItemCommand>();
            services.AddSingleton<IMapWorkItemsAndChildTasksToApiResponseModelsCommand, MapWorkItemsAndChildTasksToApiResponseModelsCommand>();
            services.AddSingleton<IMapWorkItemToParentWorkItemCommand, MapWorkItemToParentWorkItemCommand>();
            services.AddSingleton<IMapWorkItemToTaskCommand, MapWorkItemToTaskCommand>();

            // Bytes
            services.AddSingleton<IConvertAllBytesCommand, ConvertAllBytesCommand>();
            services.AddSingleton<IConvertBytesCommand, ConvertBytesCommand>();
            services.AddSingleton<IGiveGiftSubBytesCommand, GiveGiftSubBytesCommand>();
            services.AddSingleton<IGiveViewershipBytesCommand, GiveViewershipBytesCommand>();

            // Channel Rewards
            services.AddSingleton<ICreateOrUpdateChannelRewardCommand, CreateOrUpdateChannelRewardCommand>();
            services.AddSingleton<IStoreChannelRewardRedemptionCommand, StoreChannelRewardRedemptionCommand>();

            // Chat Command
            services.AddSingleton<IAddChatCommandCommand, AddChatCommandCommand>();

            // Guessing Game
            services.AddSingleton<ICompleteGuessingGameCommand, CompleteGuessingGameCommand>();
            services.AddSingleton<IGiveGuessingGameWinnersBytesCommand, GiveGuessingGameWinnersBytesCommand>();
            services.AddSingleton<ISetGuessingGameStateCommand, SetGuessingGameStateCommand>();
            services.AddSingleton<ISubmitOrUpdateGuessCommand, SubmitOrUpdateGuessCommand>();

            // Moderation
            services.AddSingleton<ITransferUserAccountCommand, TransferUserAccountCommand>();

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
            services.AddSingleton<IGiveSubscriptionVipsCommand, GiveSubscriptionVipsCommand>();
            services.AddSingleton<IModGiveVipCommand, ModGiveVipCommand>();
            services.AddSingleton<IRefundVipCommand, RefundVipCommand>();
            services.AddSingleton<IUpdateDonationVipsCommand, UpdateDonationVipsCommand>();
            services.AddSingleton<IUpdateTotalBitsCommand, UpdateTotalBitsCommand>();
            services.AddSingleton<IUseSuperVipCommand, UseSuperVipCommand>();
            services.AddSingleton<IUseVipCommand, UseVipCommand>();

            return services;
        }

        public static IServiceCollection AddApiRepositories(this IServiceCollection services)
        {
            // Bytes
            services.AddSingleton<IConvertBytesRepository, ConvertBytesRepository>();
            services.AddSingleton<IGetUserByteCountRepository, GetUserByteCountRepository>();
            services.AddSingleton<IGiveGiftSubBytesRepository, GiveGiftSubBytesRepository>();
            services.AddSingleton<IGiveUsersBytesRepository, GiveUsersBytesRepository>();
            services.AddSingleton<IGiveViewershipBytesRepository, GiveViewershipBytesRepository>();

            // Channel Rewards
            services.AddSingleton<ICreateOrUpdateChannelRewardRepository, CreateOrUpdateChannelRewardRepository>();
            services.AddSingleton<IStoreChannelRewardRedemptionRepository, StoreChannelRewardRedemptionRepository>();

            // Chat Command
            services.AddSingleton<IAddChatCommandRepository, AddChatCommandRepository>();
            services.AddSingleton<IGetCommandHelpTextByKeywordRepository, GetCommandHelpTextByKeywordRepository>();
            services.AddSingleton<IGetCommandTextByKeywordRepository, GetCommandTextByKeywordRepository>();

            // Guessing Game
            services.AddSingleton<ICloseGuessingGameRepository, CloseGuessingGameRepository>();
            services.AddSingleton<ICompleteGuessingGameRepository, CompleteGuessingGameRepository>();
            services.AddSingleton<IGetCurrentGuessingGameRepository, GetCurrentGuessingGameRepository>();
            services.AddSingleton<IGetGuessingGameStateQuery, GetGuessingGameStateQuery>();
            services.AddSingleton<IGetRunningGuessingGameIdRepository, GetRunningGuessingGameIdRepository>();
            services.AddSingleton<IGetSongPercentageGuessesRepository, GetSongPercentageGuessesRepository>();
            services.AddSingleton<IOpenGuessingGameRepository, OpenGuessingGameRepository>();
            services.AddSingleton<ISubmitOrUpdateGuessRepository, SubmitOrUpdateGuessRepository>();

            // Moderation
            services.AddSingleton<ITransferUserAccountRepository, TransferUserAccountRepository>();

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
            services.AddSingleton<IGetQuotesRepository, GetQuotesRepository>();
            services.AddSingleton<IRemoveQuoteRepository, RemoveQuoteRepository>();

            // Search
            services.AddSingleton<IGetSongBySearchIdRepository, GetSongBySearchIdRepository>();
            services.AddSingleton<IGetSongsFromSearchResultsRepository, GetSongsFromSearchResultsRepository>();
            services.AddSingleton<ISaveSearchSynonymRequestRepository, SaveSearchSynonymRequestRepository>();

            // Settings
            services.AddSingleton<IGetSettingRepository, GetSettingRepository>();
            services.AddSingleton<ISetOrCreateSettingRepository, SetOrCreateSettingRepository>();

            // StreamLabs
            services.AddSingleton<ISaveStreamLabsDonationsRepository, SaveStreamLabsDonationsRepository>();

            // Stream Status
            services.AddSingleton<IGetStreamStatusRepository, GetStreamStatusRepository>();
            services.AddSingleton<ISaveStreamStatusRepository, SaveStreamStatusRepository>();

            // Vip
            services.AddSingleton<IGetUsersCurrentSuperVipRequestCountRepository, GetUsersCurrentSuperVipRequestCountRepository>();
            services.AddSingleton<IGetUsersCurrentVipRequestCountRepository, GetUsersCurrentVipRequestCountRepository>();
            services.AddSingleton<IGetUsersGiftedVipsRepository, GetUsersGiftedVipsRepository>();
            services.AddSingleton<IGetUsersVipCountRepository, GetUsersVipCountRepository>();
            services.AddSingleton<IGiftVipRepository, GiftVipRepository>();
            services.AddSingleton<IGiveSubVipsRepository, GiveSubVipsRepository>();
            services.AddSingleton<IIsSuperVipInQueueRepository, IsSuperVipInQueueRepository>();
            services.AddSingleton<IModGiveVipRepository, ModGiveVipRepository>();
            services.AddSingleton<IRefundVipsRepository, RefundVipsRepository>();
            services.AddSingleton<IUpdateDonationVipsRepository, UpdateDonationVipsRepository>();
            services.AddSingleton<IUpdateTotalBitsRepository, UpdateTotalBitsRepository>();
            services.AddSingleton<IUseSuperVipRepository, UseSuperVipRepository>();
            services.AddSingleton<IUseVipRepository, UseVipRepository>();

            return services;
        }

        public static IServiceCollection AddSolr(this IServiceCollection services, ISecretService secretService)
        {
            var solrUser = secretService.GetSecret<string>("SolrUsername");
            var solrPass = secretService.GetSecret<string>("SolrPassword");
            var solrUrl = secretService.GetSecret<string>("SolrUrl");

            var credentials = Encoding.ASCII.GetBytes($"{solrUser}:{solrPass}");

            var credentialsBase64 = Convert.ToBase64String(credentials);

            services.AddSolrNet<SongSearch>($"{solrUrl}/SongSearch", options =>
            {
                options.HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", credentialsBase64);
            });

            services.AddSingleton<ISolrService, SolrService>();
            services.AddSingleton<IDownloadChartService, DownloadChartService>();

            return services;
        }
    }
}
