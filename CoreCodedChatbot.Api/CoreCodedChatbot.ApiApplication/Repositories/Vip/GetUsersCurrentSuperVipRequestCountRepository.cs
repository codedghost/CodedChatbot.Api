using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Vip
{
    public class GetUsersCurrentSuperVipRequestCountRepository : IGetUsersCurrentSuperVipRequestCountRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetUsersCurrentSuperVipRequestCountRepository(
            IChatbotContextFactory chatbotContextFactory
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }
        public int GetUsersCurrentSuperVipRequestCount(string username)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var superVips = context.SongRequests.Count(sr =>
                    !sr.Played && sr.Username == username && sr.SuperVipRequestTime != null);

                return superVips;
            }
        }
    }
}