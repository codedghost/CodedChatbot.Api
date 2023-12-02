using System.Linq;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Vip;

public class GetUsersCurrentSuperVipRequestCountRepository : BaseRepository<SongRequest>
{
    public GetUsersCurrentSuperVipRequestCountRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public int GetUsersCurrentSuperVipRequestCount(string username)
    {
        var superVips = Context.SongRequests.Count(sr =>
            !sr.Played && sr.Username == username && sr.SuperVipRequestTime != null);

        return superVips;
    }
}