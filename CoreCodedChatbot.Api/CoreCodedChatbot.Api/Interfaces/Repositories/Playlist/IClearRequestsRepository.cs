using System.Collections.Generic;
using CoreCodedChatbot.Api.Models.Intermediates;

namespace CoreCodedChatbot.Api.Interfaces.Repositories.Playlist
{
    public interface IClearRequestsRepository
    {
        void ClearRequests(List<BasicSongRequest> requestsToRemove);
    }
}