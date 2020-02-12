using System;
using System.Linq;
using CoreCodedChatbot.Api.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.Api.Models.Intermediates;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Library.Extensions;
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

                var users = context.Users;

                var vipRequests = unPlayedRequests.Where(sr => sr.VipRequestTime != null || sr.SuperVipRequestTime != null)
                    .OrderRequests().Select((sr, index) => FormatBasicSongRequest(users, sr, index)).ToList();

                var regularRequests = unPlayedRequests
                    .Where(sr => sr.VipRequestTime == null && sr.SuperVipRequestTime == null)
                    .OrderRequests().Select((sr, index) => FormatBasicSongRequest(users, sr, index)).ToList();

                return new CurrentRequestsIntermediate
                {
                    VipRequests = vipRequests,
                    RegularRequests = regularRequests
                };
            }
        }

        private BasicSongRequest FormatBasicSongRequest(DbSet<User> users, SongRequest songRequest, int index)
        {
            return new BasicSongRequest
            {
                SongRequestId = songRequest.SongRequestId,
                SongRequestText = songRequest.RequestText,
                Username = songRequest.RequestUsername,
                IsUserInChat = (users.SingleOrDefault(u => u.Username == songRequest.RequestUsername)?.TimeLastInChat ??
                                DateTime.MinValue)
                               .AddMinutes(2) >= DateTime.UtcNow ||
                               (songRequest.VipRequestTime ?? DateTime.MinValue).AddMinutes(5) >= DateTime.UtcNow,
                IsVip = songRequest.VipRequestTime != null,
                IsSuperVip = songRequest.SuperVipRequestTime != null,
                IsEvenIndex = index % 2 == 0,
                IsInDrive = songRequest.InDrive
            };
        }
    }
}