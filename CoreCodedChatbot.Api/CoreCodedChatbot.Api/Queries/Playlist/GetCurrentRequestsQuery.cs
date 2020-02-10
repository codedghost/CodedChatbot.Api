using CoreCodedChatbot.Api.Interfaces.Queries.Playlist;
using CoreCodedChatbot.Api.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.Api.Models.Intermediates;

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

        public CurrentRequestsIntermediate GetCurrentRequests()
        {
            return _getCurrentRequestsRepository.GetCurrentRequests();
        }
    }
}