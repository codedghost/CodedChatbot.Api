using System.Linq;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist;

public class GetUsersCurrentRegularRequestCountRepository : BaseRepository<SongRequest>
{
    public GetUsersCurrentRegularRequestCountRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public int GetUsersCurrentRegularRequestCount(string username)
    {
        var regularRequestCount = Context.SongRequests.Count(sr =>
            !sr.Played && sr.Username == username && sr.VipRequestTime == null &&
            sr.SuperVipRequestTime == null);

        return regularRequestCount;
    }
}