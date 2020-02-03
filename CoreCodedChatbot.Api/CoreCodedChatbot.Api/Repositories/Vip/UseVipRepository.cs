using System;
using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.Api.Repositories.Vip
{
    public class UseVipRepository : IUseVipRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public UseVipRepository(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void UseVip(string username, int vips)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var user = context.Users.Find(username);

                if (user == null) throw new UnauthorizedAccessException("User does not exist");

                user.UsedVipRequests += vips;

                context.SaveChanges();
            }
        }
    }
}