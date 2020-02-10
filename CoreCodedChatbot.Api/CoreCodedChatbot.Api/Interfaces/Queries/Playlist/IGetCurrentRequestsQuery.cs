using CoreCodedChatbot.Api.Models.Intermediates;

namespace CoreCodedChatbot.Api.Interfaces.Queries.Playlist
{
    public interface IGetCurrentRequestsQuery
    {
        CurrentRequestsIntermediate GetCurrentRequests();
    }
}