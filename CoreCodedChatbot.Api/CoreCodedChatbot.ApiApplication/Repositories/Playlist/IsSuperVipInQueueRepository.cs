using System.Linq;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist;

public class IsSuperVipInQueueRepository : BaseRepository<SongRequest>
{
    public IsSuperVipInQueueRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public bool IsSuperVipInQueue()
    {
        var superVip = Context.SongRequests.Where(sr => !sr.Played && sr.SuperVipRequestTime != null);

        return superVip.Any();
    }
}