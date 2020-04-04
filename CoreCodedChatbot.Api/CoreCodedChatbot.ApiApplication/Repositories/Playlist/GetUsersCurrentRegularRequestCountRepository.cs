using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist
{
    public class GetUsersCurrentRegularRequestCountRepository : IGetUsersCurrentRegularRequestCountRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetUsersCurrentRegularRequestCountRepository(
            IChatbotContextFactory chatbotContextFactory
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public int GetUsersCurrentRegularRequestCount(string username)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var regularRequestCount = context.SongRequests.Count(sr =>
                    !sr.Played && sr.RequestUsername == username && sr.VipRequestTime == null &&
                    sr.SuperVipRequestTime == null);

                return regularRequestCount;
            }
        }
    }
}