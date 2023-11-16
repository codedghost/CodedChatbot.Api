using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Queries.Playlist;

public class GetTopTenRequestsQuery : IGetTopTenRequestsQuery
{
    private readonly IGetCurrentRequestsQuery _getCurrentRequestsQuery;

    public GetTopTenRequestsQuery(
        IGetCurrentRequestsQuery getCurrentRequestsQuery
    )
    {
        _getCurrentRequestsQuery = getCurrentRequestsQuery;
    }

    public List<PlaylistItem> Get()
    {
        var currentRequests = _getCurrentRequestsQuery.GetCurrentRequests();

        return currentRequests.VipRequests.Take(10).ToList();
    }
}