using System.Collections.Generic;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist
{
    public interface IGetUsersFormattedRequestsQuery
    {
        List<string> GetUsersFormattedRequests(string username);
    }
}