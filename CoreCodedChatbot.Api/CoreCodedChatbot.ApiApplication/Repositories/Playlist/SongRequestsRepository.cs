using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using System.Linq;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist;

public class SongRequestsRepository : BaseRepository<SongRequest>
{
    public SongRequestsRepository(IChatbotContextFactory chatbotContextFactory) : base(chatbotContextFactory)
    {
    }

    public int GetUsersCurrentVipRequestCount(string username)
    {
        var vips = Context.SongRequests.Count(sr =>
            !sr.Played && sr.Username == username && sr.VipRequestTime != null &&
            sr.SuperVipRequestTime == null);

        return vips;
    }

    public int GetUsersCurrentSuperVipRequestCount(string username)
    {
        var superVips = Context.SongRequests.Count(sr =>
            !sr.Played && sr.Username == username && sr.SuperVipRequestTime != null);

        return superVips;
    }
}