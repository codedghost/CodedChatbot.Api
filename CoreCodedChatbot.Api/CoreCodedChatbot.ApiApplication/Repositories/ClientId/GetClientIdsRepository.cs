using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ClientId;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.ClientId;

public class GetClientIdsRepository : IGetClientIdsRepository
{
    private readonly IChatbotContextFactory _chatbotContextFactory;

    public GetClientIdsRepository(IChatbotContextFactory chatbotContextFactory)
    {
        _chatbotContextFactory = chatbotContextFactory;
    }

    public List<string> Get(string username, string hubType)
    {
        using (var context = _chatbotContextFactory.Create())
        {
            var user = context.Users.Find(username);

            if (user == null) return new List<string>();

            var clientIds = user.GetClientIdsDictionary();

            return clientIds.ContainsKey(hubType) ? clientIds[hubType] : new List<string>();
        }
    }
}