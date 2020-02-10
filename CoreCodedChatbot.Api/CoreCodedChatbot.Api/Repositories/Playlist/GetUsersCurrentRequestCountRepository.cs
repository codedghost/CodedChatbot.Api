using System.Linq;
using CoreCodedChatbot.Api.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.Api.Repositories.Playlist
{
    public class GetUsersCurrentRequestCountRepository : IGetUsersCurrentRequestCountRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetUsersCurrentRequestCountRepository(
            IChatbotContextFactory chatbotContextFactory
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public int GetUsersCurrentRequestCount(string username)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var requests = context.SongRequests.Count(sr => !sr.Played && sr.RequestUsername == username);

                return requests;
            }
        }
    }
}