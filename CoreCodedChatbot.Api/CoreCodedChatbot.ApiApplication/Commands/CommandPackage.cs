using CoreCodedChatbot.ApiApplication.Commands.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Commands.Bytes;
using CoreCodedChatbot.ApiApplication.Commands.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Commands.ChatCommand;
using CoreCodedChatbot.ApiApplication.Commands.ClientId;
using CoreCodedChatbot.ApiApplication.Commands.GuessingGame;
using CoreCodedChatbot.ApiApplication.Commands.Moderation;
using CoreCodedChatbot.ApiApplication.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Commands.Quote;
using CoreCodedChatbot.ApiApplication.Commands.Settings;
using CoreCodedChatbot.ApiApplication.Commands.StreamStatus;
using CoreCodedChatbot.ApiApplication.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Bytes;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.ChannelRewards;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.ChatCommand;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.ClientId;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Moderation;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Settings;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.StreamStatus;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using Microsoft.Extensions.DependencyInjection;

namespace CoreCodedChatbot.ApiApplication.Commands
{
    public static class CommandPackage
    {
        public static IServiceCollection AddApiCommands(this IServiceCollection services)
        {
            // Azure DevOps
            services
                .AddSingleton<ICreateJsonPatchDocumentFromBugRequestCommand,
                    CreateJsonPatchDocumentFromBugRequestCommand>();
            services
                .AddSingleton<ICreateJsonPatchDocumentFromProductBacklogItemRequestCommand,
                    CreateJsonPatchDocumentFromProductBacklogItemRequestCommand>();
            services.AddSingleton<ICreateJsonPatchForWorkItemCommand, CreateJsonPatchForWorkItemCommand>();
            services
                .AddSingleton<IMapWorkItemsAndChildTasksToApiResponseModelsCommand,
                    MapWorkItemsAndChildTasksToApiResponseModelsCommand>();
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

            // ClientId Command
            services.AddSingleton<IStoreClientIdCommand, StoreClientIdCommand>();
            services.AddSingleton<IRemoveClientIdCommand, RemoveClientIdCommand>();

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
            services.AddSingleton<IPromoteRequestCommand, PromoteRequestCommand>();
            services.AddSingleton<IRemoveAndRefundAllRequestsCommand, RemoveAndRefundAllRequestsCommand>();
            services.AddSingleton<IRemoveSuperVipCommand, RemoveSuperVipCommand>();
            services
                .AddSingleton<IRemoveUsersRequestByPlaylistIndexCommand, RemoveUsersRequestByPlaylistIndexCommand>();
            services.AddSingleton<IUpdatePlaylistStateCommand, UpdatePlaylistStateCommand>();

            // Quote
            services.AddSingleton<IAddQuoteCommand, AddQuoteCommand>();
            services.AddSingleton<IEditQuoteCommand, EditQuoteCommand>();
            services.AddSingleton<IRemoveQuoteCommand, RemoveQuoteCommand>();

            // Search
            services.AddSingleton<ISaveSearchSynonymRequestCommand, SaveSearchSynonymRequestCommand>();

            // Settings
            services.AddSingleton<IUpdateSettingsCommand, UpdateSettingsCommand>();

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
            services.AddSingleton<IGiveChannelPointsVipCommand, GiveChannelPointsVipCommand>();

            return services;
        }
    }
}