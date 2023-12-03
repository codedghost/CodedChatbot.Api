using CoreCodedChatbot.ApiApplication.Commands.Bytes;
using CoreCodedChatbot.ApiApplication.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Commands.Vip;
using Microsoft.Extensions.DependencyInjection;

namespace CoreCodedChatbot.ApiApplication.Commands;

public static class CommandPackage
{
    public static IServiceCollection AddApiCommands(this IServiceCollection services)
    {
        // Bytes
        services.AddSingleton<IConvertAllBytesCommand, ConvertAllBytesCommand>();
        services.AddSingleton<IConvertBytesCommand, ConvertBytesCommand>();
        services.AddSingleton<IGiveGiftSubBytesCommand, GiveGiftSubBytesCommand>();

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

        // Search
        services.AddSingleton<ISaveSearchSynonymRequestCommand, SaveSearchSynonymRequestCommand>();

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