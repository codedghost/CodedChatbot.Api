using System.Linq;
using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.Api.Repositories.Vip
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
                    !sr.Played && sr.RequestUsername == username && sr.SuperVipRequestTime != null);

                return superVips;
            }
        }
    }
}