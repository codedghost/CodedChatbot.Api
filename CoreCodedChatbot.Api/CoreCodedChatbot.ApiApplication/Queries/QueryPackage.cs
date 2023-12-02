using CoreCodedChatbot.ApiApplication.Interfaces.Queries.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Bytes;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.GuessingGame;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.StreamLabs;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.StreamStatus;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Vip;
using CoreCodedChatbot.ApiApplication.Queries.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Queries.Bytes;
using CoreCodedChatbot.ApiApplication.Queries.GuessingGame;
using CoreCodedChatbot.ApiApplication.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Queries.Search;
using CoreCodedChatbot.ApiApplication.Queries.StreamLabs;
using CoreCodedChatbot.ApiApplication.Queries.StreamStatus;
using CoreCodedChatbot.ApiApplication.Queries.Vip;
using Microsoft.Extensions.DependencyInjection;

namespace CoreCodedChatbot.ApiApplication.Queries;

public static class QueryPackage
{
    public static IServiceCollection AddApiQueries(this IServiceCollection services)
    {
        // Bytes
        services.AddSingleton<IGetUserByteCountQuery, GetUserByteCountQuery>();

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

        // Search
        services.AddSingleton<IGetPriorityChartFromSearchResultsQuery, GetPriorityChartFromSearchResultsQuery>();
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
}