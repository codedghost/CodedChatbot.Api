using System.Collections.Generic;

namespace CoreCodedChatbot.Api.Interfaces.Queries.Playlist
{
    public interface IGetUsersFormattedRequestsQuery
    {
        List<string> GetUsersFormattedRequests(string username);
    }
}