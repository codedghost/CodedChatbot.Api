using System;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiContract.Enums.Playlist;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist.ChildModels;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

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

        public AddSongResult AddRequest(string requestText, string username, bool isVip, bool isSuperVip,
            int searchSongId)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var songRequest = new SongRequest
                {
                    RequestText = requestText,
                    Username = username,
                    Played = false,
                    RequestTime = DateTime.UtcNow,
                    VipRequestTime = isVip || isSuperVip ? DateTime.UtcNow : (DateTime?) null,
                    SuperVipRequestTime = isSuperVip ? DateTime.UtcNow : (DateTime?) null,
                    InDrive = searchSongId != 0,
                    SongId = searchSongId != 0 ? searchSongId : (int?) null
                };


                context.SongRequests.Add(songRequest);
                context.SaveChanges();

                var playlistIndex = context.SongRequests.Where(sr => !sr.Played).OrderRequests()
                                        .FindIndex(sr => sr.SongRequestId == songRequest.SongRequestId) + 1;

                var formattedRequest = FormattedRequest.GetFormattedRequest(requestText);

                var song = searchSongId != 0 ? context.Songs.FirstOrDefault(s => s.SongId == searchSongId) : null;

                return new AddSongResult
                {
                    AddRequestResult = AddRequestResult.Success,
                    SongRequestId = songRequest.SongRequestId,
                    SongIndex = playlistIndex,
                    FormattedSongText = song == null ? requestText : $"{song.SongArtist} - {song.SongName} ({formattedRequest.InstrumentName})"
                };
            }
        }
    }
}