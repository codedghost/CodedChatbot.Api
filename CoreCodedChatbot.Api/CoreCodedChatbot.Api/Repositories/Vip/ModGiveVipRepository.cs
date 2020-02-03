using CoreCodedChatbot.Api.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.Api.Repositories.Vip
{
    public class ModGiveVipRepository : IModGiveVipRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public ModGiveVipRepository(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void ModGiveVip(string username, int vipsToGive)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var user = context.Users.Find(username) ?? new User
                {
                    Username = username
                };

                user.ModGivenVipRequests += vipsToGive;

                context.SaveChanges();
            }
        }
    }
}