using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist;

public class IsSuperVipInQueueRepository : IIsSuperVipInQueueRepository
{
    private readonly IChatbotContextFactory _chatbotContextFactory;

    public IsSuperVipInQueueRepository(
        IChatbotContextFactory chatbotContextFactory
    )
    {
        _chatbotContextFactory = chatbotContextFactory;
    }

    public bool IsSuperVipInQueue()
    {
        using (var context = _chatbotContextFactory.Create())
        {
            var superVip = context.SongRequests.Where(sr => !sr.Played && sr.SuperVipRequestTime != null);

            return superVip.Any();
        }
    }
}