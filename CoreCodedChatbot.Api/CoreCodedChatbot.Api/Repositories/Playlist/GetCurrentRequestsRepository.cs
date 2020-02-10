using System;
using System.Linq;
using CoreCodedChatbot.Api.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.Api.Models.Intermediates;
using CoreCodedChatbot.Database.Context.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoreCodedChatbot.Api.Repositories.Playlist
{
    public class GetCurrentRequestsRepository : IGetCurrentRequestsRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetCurrentRequestsRepository(
            IChatbotContextFactory chatbotContextFactory
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public CurrentRequestsIntermediate GetCurrentRequests()
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var unPlayedRequests = context.SongRequests.Where(sr => !sr.Played);

                var currentRequests = unPlayedRequests.Select(r => new BasicSongRequest
                {
                    SongRequestId = r.SongRequestId,
                    SongRequestText = r.RequestText,
                    Username = r.RequestUsername,
                    IsVip = r.VipRequestTime != null && r.SuperVipRequestTime != null,
                    IsUserInChat = (context.Users.SingleOrDefault(u => u.Username == r.RequestUsername)?.TimeLastInChat ?? DateTime.MinValue)
                                   .AddMinutes(2) >= DateTime.UtcNow ||
                                   (r.VipRequestTime ?? DateTime.MinValue).AddMinutes(5) >= DateTime.UtcNow,
                    IsSuperVip = r.SuperVipRequestTime != null
                }).ToList();

                return new CurrentRequestsIntermediate
                {
                    SongRequests = currentRequests
                };
            }
        }
    }
}