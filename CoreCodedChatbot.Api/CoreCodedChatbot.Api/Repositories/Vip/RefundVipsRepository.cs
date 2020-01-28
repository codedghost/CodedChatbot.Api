using System.Collections.Generic;
using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.Api.Repositories.Vip
{
    public class RefundVipsRepository : IRefundVipsRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public RefundVipsRepository(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void RefundVips(IEnumerable<string> usernames)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                foreach (var username in usernames)
                {
                    var user = context.Users.Find(username);

                    if (user == null) continue;

                    user.ModGivenVipRequests++;
                }

                context.SaveChanges();
            }
        }
    }
}