using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.Api.Repositories.Vip
{
    public class GiftVipRepository : IGiftVipRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GiftVipRepository(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void GiftVip(string donorUsername, string receivingUsername)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var donorUser = context.Users.Find(donorUsername);
                var receivingUser = context.Users.Find(receivingUsername);

                donorUser.SentGiftVipRequests++;
                receivingUser.ReceivedGiftVipRequests++;

                context.SaveChanges();
            }
        }
    }
}