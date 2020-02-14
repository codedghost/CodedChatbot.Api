using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist
{
    public interface IGetUsersRequestsRepository
    {
        List<UsersRequestsIntermediate> GetUsersRequests(string username);
    }
}