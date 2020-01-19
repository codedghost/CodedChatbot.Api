using System.Linq;
using CoreCodedChatbot.Api.Interfaces.Repositories;
using CoreCodedChatbot.Database.Context.Interfaces;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api.Repositories
{
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
}