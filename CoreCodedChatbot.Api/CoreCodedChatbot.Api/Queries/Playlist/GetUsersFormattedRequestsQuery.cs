using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.Api.Interfaces.Queries.Playlist;
using CoreCodedChatbot.Api.Interfaces.Repositories.Playlist;
using Microsoft.VisualBasic;

namespace CoreCodedChatbot.Api.Queries.Playlist
{
    public class GetUsersFormattedRequestsQuery : IGetUsersFormattedRequestsQuery
    {
        private readonly IGetUsersRequestsRepository _getUsersRequestsRepository;

        public GetUsersFormattedRequestsQuery(
            IGetUsersRequestsRepository getUsersRequestsRepository
            )
        {
            _getUsersRequestsRepository = getUsersRequestsRepository;
        }

        public List<string> GetUsersFormattedRequests(string username)
        {
            var usersRequests = _getUsersRequestsRepository.GetUsersRequests(username);

            var formattedUserRequests = usersRequests.Select(sr =>
                sr.IsVip ? $"{sr.PlaylistPosition} - {sr.SongRequestsText}" : sr.SongRequestsText).ToList();

            return formattedUserRequests;
        }
    }
}