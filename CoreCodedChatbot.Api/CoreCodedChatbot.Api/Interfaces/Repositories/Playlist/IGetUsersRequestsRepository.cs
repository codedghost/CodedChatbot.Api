using System.Collections.Generic;
using CoreCodedChatbot.Api.Models.Intermediates;

namespace CoreCodedChatbot.Api.Interfaces.Repositories.Playlist
{
    public interface IGetUsersRequestsRepository
    {
        List<UsersRequestsIntermediate> GetUsersRequests(string username);
    }
}