using System;
using System.Linq;
using System.Web;
using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist.ChildModels;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.EntityFrameworkCore;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist;

public class GetCurrentRequestsRepository : BaseRepository<SongRequest>
{
    public GetCurrentRequestsRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public CurrentRequestsIntermediate GetCurrentRequests()
    {
        var unPlayedRequests = Context.SongRequests
            .Include(sr => sr.Song)
            .Where(sr => !sr.Played);

        var users = Context.Users;

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

    private BasicSongRequest FormatBasicSongRequest(DbSet<User> users, SongRequest songRequest, int index)
    {
        var formattedRequest = FormattedRequest.GetFormattedRequest(songRequest.RequestText);
        var songRequestText = songRequest.Song == null
            ? songRequest.RequestText
            : formattedRequest == null
                ? $"{songRequest.Song.SongArtist} - {songRequest.Song.SongName} (guitar)"
                : $"{songRequest.Song.SongArtist} - {songRequest.Song.SongName} ({formattedRequest.InstrumentName})";

        return new BasicSongRequest
        {
            SongRequestId = songRequest.SongRequestId,
            SongRequestText = HttpUtility.HtmlDecode(songRequestText),
            Username = songRequest.Username,
            IsUserInChat = (users.SingleOrDefault(u => u.Username == songRequest.Username)?.TimeLastInChat ??
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