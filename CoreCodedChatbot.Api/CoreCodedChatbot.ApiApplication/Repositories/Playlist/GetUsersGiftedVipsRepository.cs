using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist
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
                var user = context.Users.Find(username);

                return user.SentGiftVipRequests;
            }
        }
    }
}