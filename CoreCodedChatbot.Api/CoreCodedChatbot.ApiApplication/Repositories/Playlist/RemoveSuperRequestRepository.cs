using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist
{
    public class RemoveSuperRequestRepository : IRemoveSuperRequestRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;
        private readonly IConfigService _configService;

        public RemoveSuperRequestRepository(
            IChatbotContextFactory chatbotContextFactory,
            IConfigService configService
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
            _configService = configService;
        }

        public void Remove(string username)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var superVip = context.SongRequests.SingleOrDefault(sr =>
                    !sr.Played &&
                    sr.SuperVipRequestTime != null &&
                    sr.RequestUsername == username
                );
                
                if (superVip == null) return;

                var user = context.Users.Find(username);

                if (user == null) return;

                var superVipCost = _configService.Get<int>("SuperVipCost");

                user.ModGivenVipRequests += superVipCost;
                superVip.Played = true;

                context.SaveChanges();
            }
        }
    }
}