using CoreCodedChatbot.ApiApplication.Commands.Playlist;
using Microsoft.Extensions.DependencyInjection;

namespace CoreCodedChatbot.ApiApplication.Commands;

public static class CommandPackage
{
    public static IServiceCollection AddApiCommands(this IServiceCollection services)
    {

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

        return services;
    }
}