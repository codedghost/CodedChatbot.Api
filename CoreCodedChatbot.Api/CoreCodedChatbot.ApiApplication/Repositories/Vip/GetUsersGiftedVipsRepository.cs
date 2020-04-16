using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Vip
{
    public class GetUsersGiftedVipsRepository : IGetUsersGiftedVipsRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetUsersGiftedVipsRepository(
            IChatbotContextFactory chatbotContextFactory
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public int GetUsersGiftedVips(string username)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var user = context.GetOrCreateUser(username);

                return user.SentGiftVipRequests;
            }
        }
    }
}