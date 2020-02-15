using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist
{
    public class ClearRequestsRepository : IClearRequestsRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public ClearRequestsRepository(
            IChatbotContextFactory chatbotContextFactory
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void ClearRequests(List<BasicSongRequest> requestsToRemove)
        {
            if (requestsToRemove == null) return;

            using (var context = _chatbotContextFactory.Create())
            {
                var songRequests =
                    context.SongRequests.Where(sr => requestsToRemove.Any(r => r.SongRequestId == sr.SongRequestId));

                foreach (var request in songRequests)
                {
                    request.Played = true;
                }

                context.SaveChanges();
            }
        }
    }
}