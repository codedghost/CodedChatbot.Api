using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Vip;

public class ModGiveVipRepository : BaseRepository<User>
{
    public ModGiveVipRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public async Task ModGiveVip(string username, int vipsToGive)
    {
        var user = Context.GetOrCreateUser(username);

        user.ModGivenVipRequests += vipsToGive;

        await Context.SaveChangesAsync();
    }
}