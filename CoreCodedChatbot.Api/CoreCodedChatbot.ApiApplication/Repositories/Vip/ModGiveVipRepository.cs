using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Database;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Vip
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
                var user = context.GetOrCreateUser(username);

                user.ModGivenVipRequests += vipsToGive;

                context.SaveChanges();
            }
        }
    }
}