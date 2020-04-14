using System;
using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Database;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Bytes
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
                    var user = context.GetOrCreateUser(winner.Username);

                    user.TokenBytes += (int) Math.Round(winner.Bytes * _configService.Get<int>("BytesToVip"));
                }

                context.SaveChanges();
            }
        }
    }
}