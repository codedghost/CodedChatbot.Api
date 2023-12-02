using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.ClientId;

public class RemoveClientIdRepository : BaseRepository<User>
{
    public RemoveClientIdRepository(IChatbotContextFactory chatbotContextFactory)
        : base(chatbotContextFactory)
    {
    }

    public void Remove(string hubType, string clientId)
    {
        Context.Users.RemoveClientId(hubType, clientId);

        Context.SaveChanges();
    }
}