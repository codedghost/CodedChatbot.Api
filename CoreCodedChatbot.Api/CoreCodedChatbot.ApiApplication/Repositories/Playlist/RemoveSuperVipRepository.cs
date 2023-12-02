using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist;

public class RemoveSuperVipRepository : BaseRepository<SongRequest>
{
    private readonly IConfigService _configService;

    public RemoveSuperVipRepository(
        IChatbotContextFactory chatbotContextFactory,
        IConfigService configService
    ) : base(chatbotContextFactory)
    {
        _configService = configService;
    }

    public async Task Remove(string username)
    {
        var superVip = Context.SongRequests.SingleOrDefault(sr =>
            !sr.Played &&
            sr.SuperVipRequestTime != null &&
            sr.Username == username
        );

        if (superVip == null) return;

        var user = await Context.Users.FindAsync(username);

        if (user == null) return;

        var superVipCost = _configService.Get<int>("SuperVipCost");

        user.ModGivenVipRequests += superVipCost;
        superVip.Played = true;

        await Context.SaveChangesAsync();
    }
}