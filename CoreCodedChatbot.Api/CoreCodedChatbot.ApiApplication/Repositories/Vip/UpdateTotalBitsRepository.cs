using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Vip
{
    public class UpdateTotalBitsRepository : IUpdateTotalBitsRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public UpdateTotalBitsRepository(
            IChatbotContextFactory chatbotContextFactory
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void Update(string username, int totalBits)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var user = context.GetOrCreateUser(username);

                user.TotalBitsDropped = totalBits;

                context.SaveChanges();
            }
        }
    }
}