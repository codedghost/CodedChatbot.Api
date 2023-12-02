using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Vip;

public class GetUsersGiftedVipsRepository : BaseRepository<User>
{
    public GetUsersGiftedVipsRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public int GetUsersGiftedVips(string username)
    {
        var user = Context.GetOrCreateUser(username);

        return user.SentGiftVipRequests;
    }
}