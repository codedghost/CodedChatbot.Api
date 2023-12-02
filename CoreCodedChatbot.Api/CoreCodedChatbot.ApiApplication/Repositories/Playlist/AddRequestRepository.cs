using System;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.ApiContract.Enums.Playlist;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist.ChildModels;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist;

public class AddRequestRepository : BaseRepository<SongRequest>
{
    public AddRequestRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public async Task<AddSongResult> AddRequest(
        string requestText,
        string username,
        bool isVip,
        bool isSuperVip,
        int searchSongId)
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

        await CreateAndSaveAsync(songRequest);

        var playlistIndex = Context.SongRequests.Where(sr => !sr.Played).OrderRequests()
            .FindIndex(sr => sr.SongRequestId == songRequest.SongRequestId) + 1;

        var formattedRequest = FormattedRequest.GetFormattedRequest(requestText);

        var song = searchSongId != 0 ? Context.Songs.FirstOrDefault(s => s.SongId == searchSongId) : null;

        return new AddSongResult
        {
            AddRequestResult = AddRequestResult.Success,
            SongRequestId = songRequest.SongRequestId,
            SongIndex = playlistIndex,
            FormattedSongText = song == null
                ? requestText
                : $"{song.SongArtist} - {song.SongName} ({formattedRequest.InstrumentName})"
        };
    }
}