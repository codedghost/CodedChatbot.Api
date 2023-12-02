using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Bytes;

public class GiveGiftSubBytesRepository : BaseRepository<User>
{
    public GiveGiftSubBytesRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public void Give(string username, int conversionAmount)
    {
        var user = Context.GetOrCreateUser(username);
        var totalBytes = conversionAmount / 2;

        user.TokenBytes += totalBytes;

        Context.SaveChanges();
    }
}