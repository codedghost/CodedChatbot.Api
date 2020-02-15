using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist
{
    public class EditSuperVipRequestRepository : IEditSuperVipRequestRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public EditSuperVipRequestRepository(
            IChatbotContextFactory chatbotContextFactory
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public int Edit(string username, string newText)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var superVip = context.SongRequests.SingleOrDefault(sr =>
                    !sr.Played &&
                    sr.SuperVipRequestTime != null &&
                    sr.RequestUsername == username
                );

                if (superVip == null) return 0;

                superVip.RequestText = newText;

                return superVip.SongId;
            }
        }
    }
}