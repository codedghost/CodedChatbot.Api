using System;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiContract.Enums.Playlist;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using CoreCodedChatbot.Library.Extensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist
{
    public class AddRequestRepository : IAddRequestRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public AddRequestRepository(
            IChatbotContextFactory chatbotContextFactory
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public AddSongResult AddRequest(string requestText, string username, bool isVip, bool isSuperVip)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var songRequest = new SongRequest
                {
                    RequestText = requestText,
                    RequestUsername = username,
                    Played = false,
                    RequestTime = DateTime.UtcNow,
                    VipRequestTime = isVip || isSuperVip ? DateTime.UtcNow : (DateTime?) null,
                    SuperVipRequestTime = isSuperVip ? DateTime.UtcNow : (DateTime?) null
                };

                context.SongRequests.Add(songRequest);
                context.SaveChanges();

                var playlistIndex = context.SongRequests.Where(sr => !sr.Played).OrderRequests()
                                        .FindIndex(sr => sr.SongRequestId == songRequest.SongRequestId) + 1;

                return new AddSongResult
                {
                    AddRequestResult = AddRequestResult.Success,
                    SongRequestId = songRequest.SongRequestId,
                    SongIndex = playlistIndex
                };
            }
        }
    }
}