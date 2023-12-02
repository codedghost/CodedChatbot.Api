using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist;

public class RemoveRegularRequestRepository : BaseRepository<SongRequest>
{
    public RemoveRegularRequestRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public async Task<bool> Remove(string username)
    {
        var usersRegularRequests = Context.SongRequests.SingleOrDefault(sr =>
            !sr.Played &&
            sr.Username == username &&
            sr.VipRequestTime == null &&
            sr.SuperVipRequestTime == null
        );

        if (usersRegularRequests == null) return false;

        usersRegularRequests.Played = true;

        await Context.SaveChangesAsync();

        return true;
    }
}