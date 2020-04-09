using System.Collections.Generic;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist
{
    public interface IGetTopTenRequestsQuery
    {
        List<PlaylistItem> Get();
    }
}