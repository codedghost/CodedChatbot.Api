using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;

public interface IClearRequestsRepository
{
    void ClearRequests(List<BasicSongRequest> requestsToRemove);
}