using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Vip;

public class GiftVipRepository : BaseRepository<User>
{
    public GiftVipRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public async Task GiftVip(string donorUsername, string receivingUsername, int vipsToGift)
    {
        var donorUser = await GetByIdAsync(donorUsername);
        var receivingUser = Context.GetOrCreateUser(receivingUsername);

        donorUser.SentGiftVipRequests += vipsToGift;
        receivingUser.ReceivedGiftVipRequests += vipsToGift;

        await Context.SaveChangesAsync();
    }
}