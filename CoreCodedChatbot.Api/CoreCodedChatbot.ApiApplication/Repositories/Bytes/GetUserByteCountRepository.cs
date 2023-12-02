using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Bytes;

public class GetUserByteCountRepository : BaseRepository<User>
{
    public GetUserByteCountRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public float Get(string username, int byteConversion)
    {
        var user = Context.GetOrCreateUser(username);

        return user.TokenBytes / (float) byteConversion;
    }
}