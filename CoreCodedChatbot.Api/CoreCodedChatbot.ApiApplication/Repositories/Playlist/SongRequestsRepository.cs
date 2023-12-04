using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Models.Enums;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.ApiContract.Enums.Playlist;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist.ChildModels;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CoreCodedChatbot.Config;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist;

public class SongRequestsRepository : BaseRepository<SongRequest>
{
    private readonly IConfigService _configService;

    public SongRequestsRepository(
        IChatbotContextFactory chatbotContextFactory,
        IConfigService configService) 
        : base(chatbotContextFactory)
    {
        _configService = configService;
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
            VipRequestTime = isVip || isSuperVip ? DateTime.UtcNow : (DateTime?)null,
            SuperVipRequestTime = isSuperVip ? DateTime.UtcNow : (DateTime?)null,
            InDrive = searchSongId != 0,
            SongId = searchSongId != 0 ? searchSongId : (int?)null
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

    public async Task<bool> AddSongToDrive(int songRequestId)
    {
        var songRequest = await GetByIdOrNullAsync(songRequestId);

        if (songRequest == null || songRequest.Played || songRequest.InDrive)
            return false;

        songRequest.InDrive = true;

        await Context.SaveChangesAsync();

        return true;
    }

    public async Task<string> ArchiveRequest(int requestId)
    {
        var request = await GetByIdOrNullAsync(requestId);

        if (request == null) return string.Empty;

        request.Played = true;

        await Context.SaveChangesAsync();

        return request.Username;
    }

    public async Task ClearRequests(List<BasicSongRequest> requestsToRemove)
    {
        if (requestsToRemove == null || !requestsToRemove.Any()) return;

        foreach (var request in requestsToRemove)
        {
            var songRequest = await GetByIdAsync(request.SongRequestId);

            songRequest.Played = true;
        }

        await Context.SaveChangesAsync();
    }

    public async Task EditRequest(int songRequestId, string requestText, string username, bool isMod, int songId)
    {
        var songRequest = await GetByIdOrNullAsync(songRequestId);

        if (songRequest == null || (songRequest.Username != username && !isMod))
            throw new UnauthorizedAccessException(
                $"{username} attempted to edit a request which was not theirs: {songRequestId}");

        songRequest.RequestText = requestText;
        songRequest.SongId = songId != 0 ? songId : (int?)null;
        songRequest.InDrive = songId != 0;
        await Context.SaveChangesAsync();
    }

    public async Task<int> EditSuperVip(string username, string newText, int songId)
    {
        var superVip = Context.SongRequests.SingleOrDefault(sr =>
            !sr.Played &&
            sr.SuperVipRequestTime != null &&
            sr.Username == username
        );

        if (superVip == null) return 0;

        superVip.RequestText = newText;
        superVip.SongId = songId != 0 ? songId : (int?)null;

        await Context.SaveChangesAsync();

        return superVip.SongRequestId;
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

    public int GetSingleSongRequestId(string username, SongRequestType songRequestType)
    {
        var songRequests = Context.SongRequests.Where(sr => sr.Username == username && !sr.Played);

        var singleRequest = songRequestType switch
        {
            SongRequestType.Regular =>
                songRequests.SingleOrDefault(sr =>
                    sr.SuperVipRequestTime == null &&
                    sr.VipRequestTime == null),
            SongRequestType.Vip =>
                songRequests.SingleOrDefault(sr =>
                    sr.SuperVipRequestTime == null &&
                    sr.VipRequestTime != null),
            SongRequestType.SuperVip =>
                songRequests.SingleOrDefault(sr =>
                    sr.SuperVipRequestTime != null),
            SongRequestType.Any => throw new ArgumentOutOfRangeException(nameof(songRequestType), songRequestType, null),
            _ => throw new ArgumentOutOfRangeException(nameof(songRequestType), songRequestType, null)
        };

        return singleRequest?.SongRequestId ?? 0;
    }

    public async Task<SongRequestIntermediate> GetRequest(int id)
    {
        var songRequest = await GetByIdOrNullAsync(id);

        if (songRequest == null) return null;

        var intermediate = new SongRequestIntermediate
        {
            SongRequestId = songRequest.SongRequestId,
            SongRequestText = songRequest.RequestText,
            SongRequestUsername = songRequest.Username,
            IsRecentRequest = songRequest.RequestTime.AddMinutes(5) >= DateTime.UtcNow,
            IsVip = songRequest.VipRequestTime != null,
            IsRecentVip = (songRequest.VipRequestTime ?? DateTime.MinValue).AddMinutes(5) >= DateTime.UtcNow,
            IsSuperVip = songRequest.SuperVipRequestTime != null,
            IsRecentSuperVip = (songRequest.SuperVipRequestTime ?? DateTime.MinValue).AddMinutes(5) >= DateTime.UtcNow,
            IsInDrive = songRequest.InDrive
        };

        return intermediate;
    }

    public int GetUsersCurrentRegularRequestCount(string username)
    {
        var regularRequestCount = Context.SongRequests.Count(sr =>
            !sr.Played && sr.Username == username && sr.VipRequestTime == null &&
            sr.SuperVipRequestTime == null);

        return regularRequestCount;
    }

    public int GetUsersCurrentRequestCount(string username)
    {
        var requests = Context.SongRequests.Count(sr => !sr.Played && sr.Username == username);

        return requests;
    }

    public List<UsersRequestsIntermediate> GetUsersRequests(string username)
    {
        var userRequests = Context.SongRequests
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

    public bool IsSuperVipInQueue()
    {
        var superVip = Context.SongRequests.Where(sr => !sr.Played && sr.SuperVipRequestTime != null);

        return superVip.Any();
    }

    public async Task<int> PromoteUserRequest(string username, int songRequestId, bool useSuperVip = false)
    {
        var request = songRequestId == 0
            ? Context.SongRequests.SingleOrDefault(sr =>
                sr.Username == username && !sr.Played && sr.VipRequestTime == null &&
                sr.SuperVipRequestTime == null)
            : Context.SongRequests.SingleOrDefault(sr =>
                sr.SongRequestId == songRequestId);

        if (request == null)
            return 0;

        request.VipRequestTime = DateTime.UtcNow;

        if (useSuperVip) request.SuperVipRequestTime = DateTime.UtcNow;

        await Context.SaveChangesAsync();

        var newSongIndex = Context.SongRequests.Where(sr => !sr.Played).OrderRequests()
            .FindIndex(sr => sr.SongRequestId == request.SongRequestId) + 1;

        return newSongIndex;
    }

    public async Task<bool> RemoveRegularRequest(string username)
    {
        var usersRegularRequests = Context.SongRequests.SingleOrDefault(sr =>
            !sr.Played &&
            sr.Username == username &&
            sr.VipRequestTime == null &&
            sr.SuperVipRequestTime == null
        );

        if (usersRegularRequests == null) return false;

        usersRegularRequests.Played = true;

        await Context.SaveChangesAsync();

        return true;
    }

    public async Task RemoveSuperVip(string username)
    {
        var superVip = Context.SongRequests.SingleOrDefault(sr =>
            !sr.Played &&
            sr.SuperVipRequestTime != null &&
            sr.Username == username
        );

        if (superVip == null) return;

        var user = await Context.Users.FindAsync(username);

        if (user == null) return;

        var superVipCost = _configService.Get<int>("SuperVipCost");

        user.ModGivenVipRequests += superVipCost;
        superVip.Played = true;

        await Context.SaveChangesAsync();
    }

    public int GetUsersCurrentVipRequestCount(string username)
    {
        var vips = Context.SongRequests.Count(sr =>
            !sr.Played && sr.Username == username && sr.VipRequestTime != null &&
            sr.SuperVipRequestTime == null);

        return vips;
    }

    public int GetUsersCurrentSuperVipRequestCount(string username)
    {
        var superVips = Context.SongRequests.Count(sr =>
            !sr.Played && sr.Username == username && sr.SuperVipRequestTime != null);

        return superVips;
    }
}