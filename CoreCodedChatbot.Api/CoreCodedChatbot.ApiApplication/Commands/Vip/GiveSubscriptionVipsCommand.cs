using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

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
                    var user = context.Users.Find(username);

                    if (user == null)
                    {
                        context.Users.Add(new User
                        {
                            Username = username,
                            ModGivenVipRequests = 0,
                            FollowVipRequest = 0,
                            DonationOrBitsVipRequests = 0,
                            SubVipRequests = 1,
                            UsedVipRequests = 0,
                            TokenBytes = 0,
                            ReceivedGiftVipRequests = 0,
                            SentGiftVipRequests = 0
                        });

                        return;
                    }

                    user.SubVipRequests++;
                }

                context.SaveChanges();
            }
        }
    }
}