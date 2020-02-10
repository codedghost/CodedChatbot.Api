using CoreCodedChatbot.Api.Interfaces.Queries.Playlist;
using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;

namespace CoreCodedChatbot.Api.Queries.Playlist
{
    public class IsSuperVipInQueueQuery : IIsSuperVipInQueueQuery
    {
        private readonly IIsSuperVipInQueueRepository _isSuperVipInQueueRepository;

        public IsSuperVipInQueueQuery(IIsSuperVipInQueueRepository isSuperVipInQueueRepository)
        {
            _isSuperVipInQueueRepository = isSuperVipInQueueRepository;
        }

        public bool IsSuperVipInQueue()
        {
            var superVipInQueue = _isSuperVipInQueueRepository.IsSuperVipInQueue();

            return superVipInQueue;
        }
    }
}