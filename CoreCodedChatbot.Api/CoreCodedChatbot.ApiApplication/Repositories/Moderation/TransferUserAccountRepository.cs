using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Moderation;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Moderation
{
    public class TransferUserAccountRepository : ITransferUserAccountRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public TransferUserAccountRepository(IChatbotContextFactory chatbotContextFactory)
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void Transfer(string moderatorUsername, string oldUsername, string newUsername)
        {
            // All users should exist int the db at this point
            using (var context = _chatbotContextFactory.Create())
            {
                context.TransferUser(moderatorUsername, oldUsername, newUsername);

                context.SaveChanges();
            }
        }
    }
}