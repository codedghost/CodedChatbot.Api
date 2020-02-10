﻿using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.Api.Repositories.Playlist
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
                var user = context.Users.Find(username);

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