using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;

namespace CoreCodedChatbot.ApiApplication.Queries.Playlist;

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