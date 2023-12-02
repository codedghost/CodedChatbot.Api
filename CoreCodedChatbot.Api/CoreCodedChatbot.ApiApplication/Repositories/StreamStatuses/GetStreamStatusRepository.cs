using System.Linq;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.StreamStatuses;

public class GetStreamStatusRepository : BaseRepository<StreamStatus>
{
    public GetStreamStatusRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public bool GetStreamStatus(string broadcasterUsername)
    {
        var status = Context.StreamStatuses.FirstOrDefault(s => s.BroadcasterUsername == broadcasterUsername);

        return status?.IsOnline ?? false;
    }
}