using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Vip
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

        public void GiftVip(string donorUsername, string receivingUsername, int vipsToGift)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var donorUser = context.Users.Find(donorUsername);
                var receivingUser = context.Users.Find(receivingUsername) ??
                                    new User
                                    {
                                        Username = receivingUsername
                                    };

                donorUser.SentGiftVipRequests += vipsToGift;
                receivingUser.ReceivedGiftVipRequests += vipsToGift;

                context.SaveChanges();
            }
        }
    }
}