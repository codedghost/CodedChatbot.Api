using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Bytes;

public class ConvertBytesRepository : BaseRepository<User>
{
    public ConvertBytesRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public int Convert(string username, int tokensToConvert, int byteConversion)
    {
            var user = Context.GetOrCreateUser(username);

            if (tokensToConvert < 0 || (user.TokenBytes < byteConversion * tokensToConvert)) return 0;

            var bytesToRemove = byteConversion * tokensToConvert;

            user.TokenBytes -= bytesToRemove;
            user.TokenVipRequests += tokensToConvert;

            Context.SaveChanges();

            return tokensToConvert;
    }
}