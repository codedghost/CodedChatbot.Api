using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist.ChildModels;
using CoreCodedChatbot.Database.Context.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist
{
    public class GetUsersRequestsRepository : IGetUsersRequestsRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetUsersRequestsRepository(
            IChatbotContextFactory chatbotContextFactory
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public List<UsersRequestsIntermediate> GetUsersRequests(string username)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var userRequests = context.SongRequests
                    .Include(sr => sr.Song)
                    .Where(sr => !sr.Played && sr.Username.ToLower() == username.ToLower())
                    .OrderRequests()
                    .Select((sr, index) =>
                        new UsersRequestsIntermediate
                        {
                            SongRequestId = sr.SongRequestId,
                            SongRequestsText = sr.Song == null
                                ? sr.RequestText
                                : $"{sr.Song.SongArtist} - {sr.Song.SongName} ({FormattedRequest.GetFormattedRequest(sr.RequestText).InstrumentName})",
                            PlaylistPosition = index + 1,
                            IsVip = sr.VipRequestTime != null || sr.SuperVipRequestTime != null,
                            IsSuperVip = sr.SuperVipRequestTime != null
                        });

                return userRequests.ToList();
            }
        }
    }
}