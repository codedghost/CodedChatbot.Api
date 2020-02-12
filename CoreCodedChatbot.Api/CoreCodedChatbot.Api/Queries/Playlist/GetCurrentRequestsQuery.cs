using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.Api.Extensions;
using CoreCodedChatbot.Api.Interfaces.Queries.Playlist;
using CoreCodedChatbot.Api.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist.ChildModels;

namespace CoreCodedChatbot.Api.Queries.Playlist
{
    public class GetCurrentRequestsQuery : IGetCurrentRequestsQuery
    {
        private readonly IGetCurrentRequestsRepository _getCurrentRequestsRepository;

        public GetCurrentRequestsQuery(
            IGetCurrentRequestsRepository getCurrentRequestsRepository
            )
        {
            _getCurrentRequestsRepository = getCurrentRequestsRepository;
        }

        public (List<PlaylistItem> RegularRequests, List<PlaylistItem> VipRequests) GetCurrentRequests()
        {
            var currentRequests = _getCurrentRequestsRepository.GetCurrentRequests();

            var regularRequests = currentRequests.RegularRequests.Select(r => r.CreatePlaylistItem()).ToList();
            var vipRequests = currentRequests.RegularRequests.Select(r => r.CreatePlaylistItem()).ToList();

            return (regularRequests, vipRequests);
        }
    }
}