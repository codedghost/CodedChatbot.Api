using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Vip;

public class GiveChannelPointsVipRepository : BaseRepository<User>
{
    public GiveChannelPointsVipRepository(IChatbotContextFactory chatbotContextFactory)
        : base(chatbotContextFactory)
    {
    }

    public async Task GiveChannelPointsVip(string username)
    {
        var user = Context.GetOrCreateUser(username);

        user.ChannelPointVipRequests++;

        await Context.SaveChangesAsync();
    }
}