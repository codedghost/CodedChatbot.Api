using System.Collections.Generic;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist.ChildModels;

namespace CoreCodedChatbot.Api.Interfaces.Queries.Playlist
{
    public interface IGetCurrentRequestsQuery
    {
        (List<PlaylistItem> RegularRequests, List<PlaylistItem> VipRequests) GetCurrentRequests();
    }
}