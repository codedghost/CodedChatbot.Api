using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ClientId;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.ClientId
{
    public class RemoveClientIdRepository : IRemoveClientIdRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public RemoveClientIdRepository(IChatbotContextFactory chatbotContextFactory)
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void Remove(string hubType, string clientId)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                context.Users.RemoveClientId(hubType, clientId);

                context.SaveChanges();
            }
        }
    }
}