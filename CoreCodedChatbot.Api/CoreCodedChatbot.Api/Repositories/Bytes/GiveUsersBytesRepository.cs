using System;
using System.Collections.Generic;
using CoreCodedChatbot.Api.Interfaces.Repositories.Bytes;
using CoreCodedChatbot.Api.Models.Intermediates;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.Api.Repositories.Bytes
{
    public class GiveUsersBytesRepository : IGiveUsersBytesRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;
        private readonly IConfigService _configService;

        public GiveUsersBytesRepository(
            IChatbotContextFactory chatbotContextFactory,
            IConfigService configService
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
            _configService = configService;
        }

        public void GiveBytes(List<GiveBytesToUserModel> users)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                foreach (var winner in users)
                {
                    // Find or add user
                    var user = context.Users.Find(winner.Username);

                    if (user == null)
                    {
                        user = new User
                        {
                            Username = winner.Username.ToLower(),
                            UsedVipRequests = 0,
                            ModGivenVipRequests = 0,
                            FollowVipRequest = 0,
                            SubVipRequests = 0,
                            DonationOrBitsVipRequests = 0,
                            TokenBytes = 0
                        };

                        context.Users.Add(user);
                    }

                    user.TokenBytes += (int) Math.Round(winner.Bytes * _configService.Get<int>("BytesToVip"));
                }

                context.SaveChanges();
            }
        }
    }
}