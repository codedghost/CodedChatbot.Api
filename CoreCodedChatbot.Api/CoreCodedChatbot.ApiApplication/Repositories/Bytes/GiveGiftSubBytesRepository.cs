using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Bytes;

public class GiveGiftSubBytesRepository : IGiveGiftSubBytesRepository
{
    private readonly IChatbotContextFactory _chatbotContextFactory;

    public GiveGiftSubBytesRepository(
        IChatbotContextFactory chatbotContextFactory
    )
    {
        _chatbotContextFactory = chatbotContextFactory;
    }

    public void Give(string username, int conversionAmount)
    {
        using (var context = _chatbotContextFactory.Create())
        {
            var user = context.GetOrCreateUser(username);
            var totalBytes = conversionAmount / 2;

            user.TokenBytes += totalBytes;

            context.SaveChanges();
        }
    }
}