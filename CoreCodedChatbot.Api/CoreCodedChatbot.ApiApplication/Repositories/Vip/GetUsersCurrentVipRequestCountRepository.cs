using System.Linq;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Vip;

public class GetUsersCurrentVipRequestCountRepository : BaseRepository<SongRequest>
{
    public GetUsersCurrentVipRequestCountRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public int GetUsersCurrentVipRequestCount(string username)
    {
        var vips = Context.SongRequests.Count(sr =>
            !sr.Played && sr.Username == username && sr.VipRequestTime != null &&
            sr.SuperVipRequestTime == null);

        return vips;
    }
}