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
            if (requestsToRemove == null || !requestsToRemove.Any()) return;

            using (var context = _chatbotContextFactory.Create())
            {
                foreach (var request in requestsToRemove)
                {
                    var songRequest = context.SongRequests.Find(request.SongRequestId);

                    songRequest.Played = true;
                }

                context.SaveChanges();
            }
        }
    }
}