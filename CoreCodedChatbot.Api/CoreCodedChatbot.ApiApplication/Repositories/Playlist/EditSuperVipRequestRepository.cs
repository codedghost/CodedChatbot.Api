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

        public int Edit(string username, string newText, int songId)
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
                superVip.SongId = songId != 0 ? songId : (int?)null;

                return superVip.SongRequestId;
            }
        }
    }
}