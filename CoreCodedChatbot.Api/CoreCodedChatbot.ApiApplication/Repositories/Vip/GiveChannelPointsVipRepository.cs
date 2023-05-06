using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Vip
{
    public class GiveChannelPointsVipRepository : IGiveChannelPointsVipRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GiveChannelPointsVipRepository(IChatbotContextFactory chatbotContextFactory)
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void GiveChannelPointsVip(string username)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var user = context.GetOrCreateUser(username);

                user.ChannelPointVipRequests++;

                context.SaveChanges();
            }
        }
    }
}