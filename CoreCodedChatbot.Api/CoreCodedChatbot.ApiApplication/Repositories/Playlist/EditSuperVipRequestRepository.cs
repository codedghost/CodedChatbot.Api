using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist;

public class EditSuperVipRequestRepository : BaseRepository<SongRequest>
{
    public EditSuperVipRequestRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public async Task<int> Edit(string username, string newText, int songId)
    {
        var superVip = Context.SongRequests.SingleOrDefault(sr =>
            !sr.Played &&
            sr.SuperVipRequestTime != null &&
            sr.Username == username
        );

        if (superVip == null) return 0;

        superVip.RequestText = newText;
        superVip.SongId = songId != 0 ? songId : (int?) null;

        await Context.SaveChangesAsync();

        return superVip.SongRequestId;
    }
}