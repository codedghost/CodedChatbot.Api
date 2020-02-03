using System;
using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.Api.Repositories.Vip
{
    public class UseSuperVipRepository : IUseSuperVipRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public UseSuperVipRepository(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void UseSuperVip(string username, int vipsToUse, int superVipsToRegister)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var user = context.Users.Find(username);

                if (user == null) throw new UnauthorizedAccessException("User does not exist");

                user.UsedSuperVipRequests += superVipsToRegister;
                user.UsedVipRequests += vipsToUse;

                context.SaveChanges();
            }
        }
    }
}