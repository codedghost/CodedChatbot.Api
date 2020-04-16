using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Commands.Vip
{
    public class GiveSubscriptionVipsCommand : IGiveSubscriptionVipsCommand
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GiveSubscriptionVipsCommand(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void Give(List<string> usernames)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                foreach (var username in usernames)
                {
                    var user = context.GetOrCreateUser(username);

                    user.SubVipRequests++;
                }

                context.SaveChanges();
            }
        }
    }
}