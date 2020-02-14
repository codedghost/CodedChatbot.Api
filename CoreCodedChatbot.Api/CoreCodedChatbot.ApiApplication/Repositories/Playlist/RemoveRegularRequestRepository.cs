using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist
{
    public class RemoveRegularRequestRepository : IRemoveRegularRequestRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public RemoveRegularRequestRepository(
            IChatbotContextFactory chatbotContextFactory
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public bool Remove(string username)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var usersRegularRequests = context.SongRequests.SingleOrDefault(sr =>
                    !sr.Played &&
                    sr.RequestUsername == username &&
                    sr.VipRequestTime == null &&
                    sr.SuperVipRequestTime == null
                );

                if (usersRegularRequests == null) return false;

                usersRegularRequests.Played = true;

                context.SaveChanges();

                return true;
            }
        }
    }
}