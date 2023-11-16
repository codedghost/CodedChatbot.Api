using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.StreamStatus;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.StreamStatus;

public class GetStreamStatusRepository : IGetStreamStatusRepository
{
    private readonly IChatbotContextFactory _chatbotContextFactory;

    public GetStreamStatusRepository(
        IChatbotContextFactory chatbotContextFactory
    )
    {
        _chatbotContextFactory = chatbotContextFactory;
    }

    public bool GetStreamStatus(string broadcasterUsername)
    {
        using (var context = _chatbotContextFactory.Create())
        {
            var status = context.StreamStatuses.FirstOrDefault(s => s.BroadcasterUsername == broadcasterUsername);

            return status?.IsOnline ?? false;
        }
    }
}