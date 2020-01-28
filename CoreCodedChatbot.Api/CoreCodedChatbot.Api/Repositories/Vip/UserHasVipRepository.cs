using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.Api.Repositories.Vip
{
    public class UserHasVipRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public UserHasVipRepository(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public bool UserHasVip(string username)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var user = context.Users.Find(username);

                if (user == null) return false;

                return 
            }
        }
    }
}