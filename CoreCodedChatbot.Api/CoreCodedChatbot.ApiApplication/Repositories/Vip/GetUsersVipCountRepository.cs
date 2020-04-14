using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Database;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Vip
{
    public class GetUsersVipCountRepository : IGetUsersVipCountRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetUsersVipCountRepository(
            IChatbotContextFactory chatbotContextFactory
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public int GetVips(string username)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var user = context.GetOrCreateUser(username);

                if (user == null) return 0;

                var vipsReceived = user.DonationOrBitsVipRequests +
                                   user.FollowVipRequest +
                                   user.ModGivenVipRequests +
                                   user.SubVipRequests +
                                   user.TokenVipRequests +
                                   user.ReceivedGiftVipRequests;

                var vipsUsed = user.UsedVipRequests + user.SentGiftVipRequests;

                return vipsReceived - vipsUsed;
            }
        }
    }
}