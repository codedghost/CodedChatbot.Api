using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Vip
{
    public class GetUsersCurrentVipRequestCountRepository : IGetUsersCurrentVipRequestCountRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetUsersCurrentVipRequestCountRepository(
            IChatbotContextFactory chatbotContextFactory
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public int GetUsersCurrentVipRequestCount(string username)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var vips = context.SongRequests.Count(sr =>
                    !sr.Played && sr.RequestUsername == username && sr.VipRequestTime != null &&
                    sr.SuperVipRequestTime == null);

                return vips;
            }
        }
    }
}