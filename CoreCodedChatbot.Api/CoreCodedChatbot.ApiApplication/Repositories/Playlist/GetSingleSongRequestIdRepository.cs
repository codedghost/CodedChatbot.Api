using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Models.Enums;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist
{
    public class GetSingleSongRequestIdRepository : IGetSingleSongRequestIdRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetSingleSongRequestIdRepository(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public int Get(string username, SongRequestType songRequestType)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var songRequests = context.SongRequests.Where(sr => sr.Username == username && !sr.Played);

                SongRequest singleRequest = null;

                switch (songRequestType)
                {
                    case SongRequestType.Regular:
                        singleRequest = songRequests.SingleOrDefault(sr =>
                            sr.SuperVipRequestTime == null &&
                            sr.VipRequestTime == null);
                        break;
                    case SongRequestType.Vip:
                        singleRequest = songRequests.SingleOrDefault(sr =>
                            sr.SuperVipRequestTime == null &&
                            sr.VipRequestTime != null);
                        break;
                    case SongRequestType.SuperVip:
                        singleRequest = songRequests.SingleOrDefault(sr =>
                            sr.SuperVipRequestTime != null);
                        break;
                }

                return singleRequest?.SongRequestId ?? 0;
            }
        }
    }
}