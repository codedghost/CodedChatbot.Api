using CoreCodedChatbot.Api.Models.Enums;

namespace CoreCodedChatbot.Api.Interfaces.Queries.Playlist
{
    public interface IGetUsersCurrentRequestCountsQuery
    {
        int GetUsersCurrentRequestCounts(string username, SongRequestType songRequestType);
    }
}