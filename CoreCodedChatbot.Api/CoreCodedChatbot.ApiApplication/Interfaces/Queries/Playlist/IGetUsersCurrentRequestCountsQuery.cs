using CoreCodedChatbot.ApiApplication.Models.Enums;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;

public interface IGetUsersCurrentRequestCountsQuery
{
    int GetUsersCurrentRequestCounts(string username, SongRequestType songRequestType);
}