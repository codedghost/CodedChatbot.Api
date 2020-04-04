using System;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist
{
    public class PromoteUserRequestRepository : IPromoteUserRequestRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public PromoteUserRequestRepository(
            IChatbotContextFactory chatbotContextFactory
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public int PromoteUserRequest(string username, int songRequestId)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                SongRequest request;
                if (songRequestId == 0)
                    request = context.SongRequests.SingleOrDefault(sr =>
                        sr.RequestUsername == username && !sr.Played && sr.VipRequestTime == null &&
                        sr.SuperVipRequestTime == null);
                else
                    request = context.SongRequests.SingleOrDefault(sr =>
                        sr.SongRequestId == songRequestId);

                if (request == null)
                    return 0;

                request.VipRequestTime = DateTime.UtcNow;

                context.SaveChanges();

                var newSongIndex = context.SongRequests.Where(sr => !sr.Played).OrderRequests()
                                       .FindIndex(sr => sr.SongRequestId == request.SongRequestId) + 1;

                return newSongIndex;
            }
        }
    }
}