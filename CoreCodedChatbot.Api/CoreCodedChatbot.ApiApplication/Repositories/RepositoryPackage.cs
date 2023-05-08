using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChatCommand;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ClientId;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Moderation;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Settings;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.StreamLabs;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.StreamStatus;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.ApiApplication.Queries.GuessingGame;
using CoreCodedChatbot.ApiApplication.Repositories.Bytes;
using CoreCodedChatbot.ApiApplication.Repositories.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Repositories.ChatCommand;
using CoreCodedChatbot.ApiApplication.Repositories.ClientId;
using CoreCodedChatbot.ApiApplication.Repositories.GuessingGame;
using CoreCodedChatbot.ApiApplication.Repositories.Moderation;
using CoreCodedChatbot.ApiApplication.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Repositories.Quote;
using CoreCodedChatbot.ApiApplication.Repositories.Search;
using CoreCodedChatbot.ApiApplication.Repositories.Settings;
using CoreCodedChatbot.ApiApplication.Repositories.StreamLabs;
using CoreCodedChatbot.ApiApplication.Repositories.StreamStatus;
using CoreCodedChatbot.ApiApplication.Repositories.Vip;
using Microsoft.Extensions.DependencyInjection;

namespace CoreCodedChatbot.ApiApplication.Repositories
{
    public static class RepositoryPackage
    {
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
            services.AddSingleton<IGetChannelRewardRepository, GetChannelRewardRepository>();

            // Chat Command
            services.AddSingleton<IAddChatCommandRepository, AddChatCommandRepository>();
            services.AddSingleton<IGetCommandHelpTextByKeywordRepository, GetCommandHelpTextByKeywordRepository>();
            services.AddSingleton<IGetCommandTextByKeywordRepository, GetCommandTextByKeywordRepository>();

            // ClientId
            services.AddSingleton<IStoreClientIdRepository, StoreClientIdRepository>();
            services.AddSingleton<IRemoveClientIdRepository, RemoveClientIdRepository>();
            services.AddSingleton<IGetClientIdsRepository, GetClientIdsRepository>();

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
            services
                .AddSingleton<IGetUsersCurrentRegularRequestCountRepository,
                    GetUsersCurrentRegularRequestCountRepository>();
            services.AddSingleton<IGetUsersCurrentRequestCountRepository, GetUsersCurrentRequestCountRepository>();
            services.AddSingleton<IGetUsersRequestsRepository, GetUsersRequestsRepository>();
            services.AddSingleton<IPromoteUserRequestRepository, PromoteUserRequestRepository>();
            services.AddSingleton<IRemoveRegularRequestRepository, RemoveRegularRequestRepository>();
            services.AddSingleton<IRemoveSuperVipRepository, RemoveSuperVipRepository>();

            // Quote
            services.AddTransient<IQuoteRepository, QuoteRepository>();

            // Search
            services
                .AddSingleton<IGetPriorityChartFromSearchResultsRepository,
                    GetPriorityChartFromSearchResultsRepository>();
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
            services
                .AddSingleton<IGetUsersCurrentSuperVipRequestCountRepository,
                    GetUsersCurrentSuperVipRequestCountRepository>();
            services
                .AddSingleton<IGetUsersCurrentVipRequestCountRepository, GetUsersCurrentVipRequestCountRepository>();
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
            services.AddSingleton<IGiveChannelPointsVipRepository, GiveChannelPointsVipRepository>();

            return services;
        }
    }
}