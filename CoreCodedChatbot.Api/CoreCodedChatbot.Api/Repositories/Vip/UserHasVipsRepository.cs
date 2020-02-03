using CoreCodedChatbot.Api.Extensions;
using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.Api.Repositories.Vip
{
    public class UserHasVipsRepository : IUserHasVipsRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public UserHasVipsRepository(
            IChatbotContextFactory chatbotContextFactory
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public bool HasVips(string username, int vips)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var user = context.Users.Find(username);

                if (user == null) return false;

                var userVips = user.UsableVips();

                return userVips >= vips;
            }
        }
    }
}