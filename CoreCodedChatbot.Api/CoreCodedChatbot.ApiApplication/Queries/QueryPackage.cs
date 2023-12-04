using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Vip;
using CoreCodedChatbot.ApiApplication.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Queries.Vip;
using Microsoft.Extensions.DependencyInjection;

namespace CoreCodedChatbot.ApiApplication.Queries;

public static class QueryPackage
{
    public static IServiceCollection AddApiQueries(this IServiceCollection services)
    {
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

        // Vip
        services.AddSingleton<ICheckUserHasVipsQuery, CheckUserHasVipsQuery>();
        services.AddSingleton<IGetUsersGiftedVipsQuery, GetUsersGiftedVipsQuery>();
        services.AddSingleton<IGetUserVipCountQuery, GetUserVipCountQuery>();

        return services;
    }
}