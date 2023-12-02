using System;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist;

public class PromoteUserRequestRepository : BaseRepository<SongRequest>
{
    public PromoteUserRequestRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public async Task<int> PromoteUserRequest(string username, int songRequestId, bool useSuperVip = false)
    {
        var request = songRequestId == 0
            ? Context.SongRequests.SingleOrDefault(sr =>
                sr.Username == username && !sr.Played && sr.VipRequestTime == null &&
                sr.SuperVipRequestTime == null)
            : Context.SongRequests.SingleOrDefault(sr =>
                sr.SongRequestId == songRequestId);

        if (request == null)
            return 0;

        request.VipRequestTime = DateTime.UtcNow;

        if (useSuperVip) request.SuperVipRequestTime = DateTime.UtcNow;

        await Context.SaveChangesAsync();

        var newSongIndex = Context.SongRequests.Where(sr => !sr.Played).OrderRequests()
            .FindIndex(sr => sr.SongRequestId == request.SongRequestId) + 1;

        return newSongIndex;
    }
}