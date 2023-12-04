using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Enums;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiApplication.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Repositories.Settings;
using CoreCodedChatbot.ApiApplication.Repositories.Users;
using CoreCodedChatbot.ApiContract.Enums.Playlist;
using CoreCodedChatbot.ApiContract.RequestModels.Playlist;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist.ChildModels;
using CoreCodedChatbot.ApiContract.SignalRHubModels.API;
using CoreCodedChatbot.ApiContract.SignalRHubModels.Website;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Secrets;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.ApiApplication.Services;

public class PlaylistService : IBaseService, IPlaylistService
{
    private readonly IChatbotContextFactory _chatbotContextFactory;
    private readonly IVipService _vipService;
    private readonly ISecretService _secretService;
    private readonly IConfigService _configService;
    private readonly ISignalRService _signalRService;
    private readonly ISearchService _searchService;
    private readonly ILogger<IPlaylistService> _logger;

    private PlaylistItem _currentRequest;
    private Random _rand;

    private int _currentVipRequestsPlayed = 0;
    private int _concurrentVipSongsToPlay = 0;

    public PlaylistService(
        IChatbotContextFactory chatbotContextFactory,
        IVipService vipService,
        ISecretService secretService,
        IConfigService configService,
        ISignalRService signalRService,
        ISearchService searchService,
        ILogger<IPlaylistService> logger
    )
    {
        _chatbotContextFactory = chatbotContextFactory;
        _vipService = vipService;
        _secretService = secretService;
        _configService = configService;
        _signalRService = signalRService;
        _searchService = searchService;
        _logger = logger;

        _concurrentVipSongsToPlay = configService.Get<int>("ConcurrentVipSongsToPlay");

        _rand = new Random();
    }

    public async Task<PlaylistItem> GetRequestById(int songId)
    {
        var playlistItem = await GetSongRequestById(songId);

        return playlistItem;
    }

    public async Task<(AddRequestResult, int)> AddRequest(string username, string commandText, bool vipRequest = false)
    {
        var songId = await _searchService.FindChartAndDownload(commandText).ConfigureAwait(false);

        var result = await AddSongRequest(username, commandText,
            vipRequest ? SongRequestType.Vip : SongRequestType.Regular, songId).ConfigureAwait(false);

        if (_currentRequest == null)
        {
            _currentRequest = new PlaylistItem
            {
                songRequestId = result.SongRequestId,
                songRequestText = result.FormattedSongText,
                songRequester = username,
                isEvenIndex = false,
                isInChat = true,
                isVip = vipRequest,
                isSuperVip = false,
                isInDrive = false
            };
        }

        UpdateFullPlaylist();


        return (result.AddRequestResult, result.SongIndex);
    }

    public async Task<AddRequestResult> AddWebRequest(AddWebSongRequest requestSongViewModel, string username)
    {
        var requestText =
            $"{requestSongViewModel.Artist} - {requestSongViewModel.Title} ({requestSongViewModel.SelectedInstrument})";

        var songId = await _searchService.FindChartAndDownload(requestText).ConfigureAwait(false);

        var result = await AddSongRequest(username, requestText,
            requestSongViewModel.IsSuperVip ? SongRequestType.SuperVip :
            requestSongViewModel.IsVip ? SongRequestType.Vip : SongRequestType.Regular, songId).ConfigureAwait(false);


        if (_currentRequest == null)
        {
            _currentRequest = new PlaylistItem
            {
                songRequestId = result.SongRequestId,
                songRequestText = result.FormattedSongText,
                songRequester = username,
                isEvenIndex = false,
                isInChat = true,
                isVip = requestSongViewModel.IsVip,
                isSuperVip = requestSongViewModel.IsSuperVip,
                isInDrive = false
            };
        }

        if (result.AddRequestResult == AddRequestResult.Success)
            UpdateFullPlaylist();

        return result.AddRequestResult;
    }

    public PlaylistState GetPlaylistState()
    {
        using (var repo = new SettingsRepository(_chatbotContextFactory))
        {
            var state = repo.Get<string>("PlaylistStatus");
            return string.IsNullOrWhiteSpace(state)
                ? PlaylistState.VeryClosed
                : Enum.Parse<PlaylistState>(state);
        }
    }

    public async Task<PromoteSongResponse> PromoteRequest(string username, int songId, bool useSuperVip)
    {
        var result = await Promote(username, useSuperVip, songId)
            .ConfigureAwait(false);

        UpdateFullPlaylist();

        return new PromoteSongResponse
        {
            PlaylistIndex = result.PlaylistIndex,
            PromoteRequestResult = result.PromoteRequestResult
        };
    }

    public async Task<PromoteRequestResult> PromoteWebRequest(int songId, string username)
    {
        var result = await Promote(username, false, songId).ConfigureAwait(false);

        UpdateFullPlaylist();

        return result.PromoteRequestResult;
    }

    public async Task ArchiveCurrentRequest(int songId = 0)
    {
        var currentRequest = songId == 0 ? _currentRequest :
            songId == _currentRequest?.songRequestId ? _currentRequest : null;

        if (currentRequest == null) return;

        await ArchiveRequest(currentRequest.songRequestId, false);

        UpdateFullPlaylist(true);
    }

    public GetAllSongsResponse GetAllSongs()
    {
        CurrentRequestsIntermediate currentRequests;
        using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
        {
            currentRequests = repo.GetCurrentRequests();
        }

        var regularRequests = currentRequests.RegularRequests.Select(r => r.CreatePlaylistItem()).ToArray();
        var vipRequests = currentRequests.VipRequests.Select(r => r.CreatePlaylistItem()).ToArray();
        
        // Ensure if the playlist is populated then a request is made current
        if (_currentRequest == null)
        {
            if (vipRequests.Any())
            {
                _currentRequest = vipRequests.First();

                vipRequests = vipRequests.Where(r => r.songRequestId != _currentRequest.songRequestId).ToArray();
            }
            else if (currentRequests.RegularRequests.Any())
            {
                _currentRequest =
                    regularRequests[_rand.Next(0, currentRequests.RegularRequests.Count)];
                regularRequests = regularRequests.Where(r => r.songRequestId != _currentRequest.songRequestId).ToArray();
            }
        }

        return new GetAllSongsResponse
        {
            CurrentSong = _currentRequest,
            RegularList = regularRequests,
            VipList = vipRequests
        };
    }

    public async Task<bool> ArchiveRequestById(int songId)
    {
        await ArchiveRequest(songId, true).ConfigureAwait(false);

        UpdateFullPlaylist();

        return true;
    }

    public async Task ClearRockRequests()
    {
        CurrentRequestsIntermediate currentRequests;
        using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
        {
            currentRequests = repo.GetCurrentRequests();
        }

        if (currentRequests == null)
            return;

        var refundVips = currentRequests?.VipRequests?.Where(sr => sr.IsSuperVip || sr.IsVip).Select(sr =>
            new VipRefund
            {
                Username = sr.Username,
                VipsToRefund = sr.IsSuperVip ? _configService.Get<int>("SuperVipCost") :
                    sr.IsVip ? 1 :
                    0
            }).ToList() ?? new List<VipRefund>();

        using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
        {
            await repo.RefundVips(refundVips);
        }

        foreach (var refund in refundVips)
        {
            await _vipService.UpdateClientVips(refund.Username).ConfigureAwait(false);
        }

        using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
        {
            await repo.ClearRequests(currentRequests.RegularRequests);
            await repo.ClearRequests(currentRequests.VipRequests);
        }

        UpdateFullPlaylist();
    }

    public async Task<bool> RemoveRockRequests(string username, string commandText, bool isMod)
    {
        bool success = false;

        // If the command text doesn't parse, we should attempt to remove a regular request
        if (!int.TryParse(commandText.Trim(), out var playlistIndex))
        {
            using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
            {
                // remove regular request if it exists
                if (repo.GetUsersCurrentRegularRequestCount(username) == 1)
                {
                    success = await ArchiveAndRefundVips(username, SongRequestType.Regular, _currentRequest.songRequestId)
                        .ConfigureAwait(false);
                }

                // if true return, otherwise attempt to remove and refund a single vip
                else if (repo.GetUsersCurrentVipRequestCount(username) == 1)
                {
                    success = await ArchiveAndRefundVips(username, SongRequestType.Vip, _currentRequest.songRequestId)
                        .ConfigureAwait(false);
                }

                else if (repo.GetUsersCurrentSuperVipRequestCount(username) == 1)
                {
                    success = await ArchiveAndRefundVips(username, SongRequestType.SuperVip, _currentRequest.songRequestId)
                        .ConfigureAwait(false);
                }

                return success;
            }
        }

        // Remove VIP request using provided playlistIndex.
        // Query can use existing GetUsersRequestsRepository to get the song request id
        success = await RemoveByPlaylistIndex(username, playlistIndex).ConfigureAwait(false);

        UpdateFullPlaylist();

        return success;
    }

    public string GetUserRequests(string username)
    {
        List<UsersRequestsIntermediate> usersRequests;
        using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
        {
            usersRequests = repo.GetUsersRequests(username);
        }

        var formattedRequests = usersRequests.Select(sr =>
            sr.IsVip ? $"{sr.PlaylistPosition} - {sr.SongRequestsText}" : sr.SongRequestsText).ToList();

        var requestString = formattedRequests.Any()
            ? string.Join(", ", formattedRequests)
            : "Looks like you don't have any requests right now. GetCommandText some in there! !howtorequest";

        return requestString;
    }

    public async Task<(bool Success, string SongRequestText, bool SyntaxError)> EditRequest(string username, string commandText, bool isMod)
    {
        var songRequestText = string.Empty;
        var syntaxError = false;
        if (string.IsNullOrWhiteSpace(commandText))
        {
            syntaxError = true;
            return (false, songRequestText, syntaxError);
        }

        var commandTextTerms = commandText.Trim().Split(" ");

        SongRequestType editRequestType;
        EditRequestResult result;

        // If the command text doesn't parse, we should attempt to edit a regular request
        if (!int.TryParse(commandTextTerms[0].Trim(), out var playlistIndex))
        {
            songRequestText = commandText;

            using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
            {
                // edit regular request if it exists
                if (repo.GetUsersCurrentRegularRequestCount(username) == 1)
                {
                    editRequestType = SongRequestType.Regular;
                }
                // if true return, otherwise attempt to edit and refund a single vip
                else if (repo.GetUsersCurrentVipRequestCount(username) == 1)
                {
                    editRequestType = SongRequestType.Vip;
                }
                else if (repo.GetUsersCurrentSuperVipRequestCount(username) == 1)
                {
                    editRequestType = SongRequestType.SuperVip;
                }
                else
                {
                    songRequestText = string.Empty;
                    syntaxError = true;
                    return (false, songRequestText, syntaxError);
                }
            }

            var formattedRequest =
                FormattedRequest.GetFormattedRequest(songRequestText);

            int songRequestId;
            using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
            {
                songRequestId = repo.GetSingleSongRequestId(username, editRequestType);
            }

            var editRequestModel = new EditWebRequestRequestModel
            {
                SongRequestId = songRequestId,
                Title = formattedRequest?.SongName ?? songRequestText,
                Artist = formattedRequest?.SongArtist ?? string.Empty,
                SelectedInstrument = formattedRequest?.InstrumentName ?? string.Empty,
                IsVip = editRequestType == SongRequestType.Vip,
                IsSuperVip = editRequestType == SongRequestType.SuperVip,
                Username = username,
                IsMod = isMod
            };

            result = await EditWebRequest(editRequestModel).ConfigureAwait(false);
        }
        else
        {
            songRequestText = string.Join(' ', commandTextTerms);

            // Edit request at position playlistIndex
            BasicSongRequest? songRequest;
            using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
            {
                var currentRequests = repo.GetCurrentRequests();

                var requestAtIndex = currentRequests.VipRequests.Where((sr, position) =>
                    sr.Username == username && _currentRequest.isSuperVip || _currentRequest.isVip ? playlistIndex == position : playlistIndex - 1 == position);

                songRequest = requestAtIndex.SingleOrDefault();
            }

            if (songRequest == null)
            {
                syntaxError = true;
                songRequestText = string.Empty;
                return (false, songRequestText, syntaxError);
            }

            var formattedRequest =
                FormattedRequest.GetFormattedRequest(commandText);
            var editRequestModel = new EditWebRequestRequestModel
            {
                SongRequestId = songRequest.SongRequestId,
                Title = formattedRequest?.SongName ?? songRequestText,
                Artist = formattedRequest?.SongArtist ?? string.Empty,
                SelectedInstrument = formattedRequest?.InstrumentName ?? string.Empty,
                IsVip = songRequest.IsVip,
                IsSuperVip = songRequest.IsSuperVip,
                Username = username,
                IsMod = isMod
            };

            result = await EditWebRequest(editRequestModel).ConfigureAwait(false);
        }

        switch (result)
        {
            case EditRequestResult.NoRequestEntered:
            case EditRequestResult.NotYourRequest:
            case EditRequestResult.RequestAlreadyRemoved:
                syntaxError = true;
                break;
            default:
                syntaxError = false;
                break;
        }

        return (result == EditRequestResult.Success, songRequestText, syntaxError);
    }

    public async Task<EditRequestResult> EditWebRequest(EditWebRequestRequestModel editWebRequestRequestModel)
    {
        if (editWebRequestRequestModel == null) return EditRequestResult.NoRequestEntered;

        try
        {
            var searchTerms =
                string.IsNullOrWhiteSpace(editWebRequestRequestModel.Artist) &&
                string.IsNullOrWhiteSpace(editWebRequestRequestModel.SelectedInstrument)
                    ? editWebRequestRequestModel.Title
                    : $"{editWebRequestRequestModel.Artist} - {editWebRequestRequestModel.Title} ({editWebRequestRequestModel.SelectedInstrument})";

            var songId = await _searchService.FindChartAndDownload(searchTerms).ConfigureAwait(false);

            using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
            {
                await repo.EditRequest(editWebRequestRequestModel.SongRequestId,
                    string.IsNullOrWhiteSpace(editWebRequestRequestModel.Artist) && string.IsNullOrWhiteSpace(editWebRequestRequestModel.SelectedInstrument)
                        ? editWebRequestRequestModel.Title
                        : $"{editWebRequestRequestModel.Artist} - {editWebRequestRequestModel.Title} ({editWebRequestRequestModel.SelectedInstrument})",
                    editWebRequestRequestModel.Username, editWebRequestRequestModel.IsMod, songId);
            }

            UpdateFullPlaylist();

            return EditRequestResult.Success;
        }
        catch (UnauthorizedAccessException)
        {
            return EditRequestResult.NotYourRequest;
        }
        catch (Exception)
        {
            return EditRequestResult.UnSuccessful;
        }
    }
    public async Task<bool> AddSongToDrive(int songId)
    {
        using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
        {
            var result = songId > 0 && await repo.AddSongToDrive(songId);
            UpdateFullPlaylist();

            return result;
        }
    }

    public async Task<bool> OpenPlaylist()
    {
        using (var repo = new SettingsRepository(_chatbotContextFactory))
        {
            await repo.Set("PlaylistStatus", "Open");
        }

        await _signalRService.UpdatePlaylistState(PlaylistState.Open).ConfigureAwait(false);

        return true;
    }

    public async Task<bool> ClosePlaylist()
    {
        using (var repo = new SettingsRepository(_chatbotContextFactory))
        {
            await repo.Set("PlaylistStatus", "Closed");
        }

        await _signalRService.UpdatePlaylistState(PlaylistState.Closed).ConfigureAwait(false);

        return true;
    }

    public async Task<bool> VeryClosePlaylist()
    {

        using (var repo = new SettingsRepository(_chatbotContextFactory))
        {
            await repo.Set("PlaylistStatus", "VeryClosed");
        }

        await _signalRService.UpdatePlaylistState(PlaylistState.VeryClosed).ConfigureAwait(false);

        return true;
    }

    public int GetMaxUserRequests()
    {
        return _configService.Get<int>("MaxRegularSongsPerUser");
    }

    public async Task<AddRequestResult> AddSuperVipRequest(string username, string commandText)
    {
        var songId = await _searchService.FindChartAndDownload(commandText).ConfigureAwait(false);

        var result = await AddSongRequest(username, commandText, SongRequestType.SuperVip, songId);

        if (_currentRequest == null)
        {
            _currentRequest = new PlaylistItem
            {
                songRequestId = result.SongRequestId,
                songRequestText = commandText,
                songRequester = username,
                isEvenIndex = false,
                isInChat = true,
                isVip = true,
                isSuperVip = true,
                isInDrive = false
            };
        }

        UpdateFullPlaylist();

        return result.AddRequestResult;
    }

    public async Task<bool> EditSuperVipRequest(string username, string songText)
    {
        var songId = await _searchService.FindChartAndDownload(songText).ConfigureAwait(false);

        int songRequestId;
        using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
        {
            songRequestId = await repo.EditSuperVip(username, songText, songId);
        }

        UpdateFullPlaylist();

        return songRequestId > 0;
    }

    public async Task<bool> RemoveSuperRequest(string username)
    {
        if (string.IsNullOrWhiteSpace(username)) return false;

        using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
        {
            await repo.RemoveSuperVip(username);

            await _vipService.UpdateClientVips(username).ConfigureAwait(false);
        }

        UpdateFullPlaylist();

        return true;
    }

    public async void UpdateFullPlaylist(bool updateCurrent = false)
    {
        var psk = _secretService.GetSecret<string>("SignalRKey");

        var connection = _signalRService.GetCurrentConnection(APIHubConstants.SongListHubPath);

        if (connection == null) return;

        var requests = GetAllSongs();

        if (updateCurrent)
        {
            UpdateCurrentSong(requests.RegularList, requests.VipList);
        }

        // Should refresh the current song object to ensure any changes (like in-drive) are respected
        var songRequestId = _currentRequest?.songRequestId ?? 0;
        var current = requests.RegularList.SingleOrDefault(r => r.songRequestId == songRequestId);
        _currentRequest = current ?? requests.VipList.SingleOrDefault(r => r.songRequestId == songRequestId);

        requests.RegularList = requests.RegularList.Where(r => r.songRequestId != songRequestId)
            .ToArray();
        requests.VipList = requests.VipList.Where(r => r.songRequestId != songRequestId).ToArray();

        await connection.InvokeAsync<SongListHubModel>("SendAll",
            new SongListHubModel
            {
                psk = psk,
                currentSong = _currentRequest,
                regularRequests = requests.RegularList,
                vipRequests = requests.VipList
            });

        await connection.InvokeAsync<SongListHubModel>("UpdateClients",
            new SongListHubModel
            {
                psk = psk,
                currentSong = _currentRequest,
                regularRequests = requests.RegularList,
                vipRequests = requests.VipList
            });
    }

    public bool IsSuperVipRequestInQueue()
    {
        using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
        {
            return repo.IsSuperVipInQueue();
        }
    }

    public PlaylistItem GetCurrentSongRequest()
    {
        return _currentRequest;
    }

    public List<PlaylistItem> GetTopTenPlaylistItems()
    {
        CurrentRequestsIntermediate currentRequests;
        using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
        {
            currentRequests = repo.GetCurrentRequests();
        }

        var vipRequests = currentRequests.VipRequests.Select(r => r.CreatePlaylistItem()).ToList();

        return vipRequests.Take(10).ToList();
    }

    private void UpdateCurrentSong(PlaylistItem[] regularRequests, PlaylistItem[] vipRequests)
    {
        var inChatRegularRequests = regularRequests.Where(r => r.isInChat).ToList();
        if (!inChatRegularRequests.Any() && !vipRequests.Any())
        {
            _currentRequest = null;
            return;
        }

        if (_currentRequest.isVip)
        {
            _currentVipRequestsPlayed++;
            if (vipRequests.Any(vr => vr.isSuperVip))
            {
                UpdateCurrentToSuperVipRequest(vipRequests);
            }
            else if (_currentVipRequestsPlayed < _concurrentVipSongsToPlay
                     && vipRequests.Any())
            {
                UpdateCurrentToVipRequest(vipRequests);
            }
            else if (inChatRegularRequests.Any())
            {
                _currentVipRequestsPlayed = 0;
                UpdateCurrentToRegularRequest(inChatRegularRequests);
            }
            else if (vipRequests.Any())
            {
                UpdateCurrentToVipRequest(vipRequests);
            }
            else
            {
                EmptyCurrent();
            }
        }
        else
        {
            if (vipRequests.Any(vr => vr.isSuperVip))
            {
                UpdateCurrentToSuperVipRequest(vipRequests);
            }
            else if (vipRequests.Any())
            {
                UpdateCurrentToVipRequest(vipRequests);
            }
            else if (inChatRegularRequests.Any())
            {
                UpdateCurrentToRegularRequest(inChatRegularRequests);
            }
            else
            {
                EmptyCurrent();
            }
        }
    }

    private void UpdateCurrentToRegularRequest(List<PlaylistItem> inChatRegularRequests)
    {
        _currentRequest = inChatRegularRequests[_rand.Next(inChatRegularRequests.Count)];
    }

    private void UpdateCurrentToVipRequest(PlaylistItem[] vipRequests)
    {

        _currentRequest = vipRequests.FirstOrDefault();
    }

    private void UpdateCurrentToSuperVipRequest(PlaylistItem[] vipRequests)
    {

        _currentRequest = vipRequests.FirstOrDefault(vr => vr.isSuperVip);
    }

    private void EmptyCurrent()
    {
        _currentRequest = null;
    }

    private async Task<PlaylistItem> GetSongRequestById(int id)
    {
        SongRequestIntermediate songRequest;
        using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
        {
            songRequest = await repo.GetRequest(id);
        }

        bool userInChat;
        using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
        {
            userInChat = await repo.IsUserInChat(songRequest.SongRequestUsername);
        }

        return new PlaylistItem
        {
            songRequestId = songRequest.SongRequestId,
            songRequestText = songRequest.SongRequestText,
            songRequester = songRequest.SongRequestUsername,
            isInChat = userInChat || songRequest.IsRecentRequest || songRequest.IsRecentVip ||
                       songRequest.IsRecentSuperVip,
            isVip = songRequest.IsVip,
            isSuperVip = songRequest.IsSuperVip,
            isInDrive = songRequest.IsInDrive
        };
    }

    private async Task<AddSongResult> AddSongRequest(string username, string requestText,
        SongRequestType songRequestType, int searchSongId)
    {
        if (string.IsNullOrWhiteSpace(requestText))
            return new AddSongResult
            {
                AddRequestResult = AddRequestResult.NoRequestEntered
            };

        if (string.IsNullOrWhiteSpace(username) || songRequestType == SongRequestType.Any)
            return new AddSongResult
            {
                AddRequestResult = AddRequestResult.UnSuccessful
            };

        PlaylistState playlistState;
        using (var repo = new SettingsRepository(_chatbotContextFactory))
        {
            var state = repo.Get<string>("PlaylistStatus");
            playlistState = string.IsNullOrWhiteSpace(state)
                ? PlaylistState.VeryClosed
                : Enum.Parse<PlaylistState>(state);
        }

        switch (playlistState)
        {
            case PlaylistState.VeryClosed:
                if (songRequestType != SongRequestType.SuperVip)
                {
                    return new AddSongResult
                    {
                        AddRequestResult = AddRequestResult.PlaylistVeryClosed
                    };
                }

                break;
            case PlaylistState.Closed:
                if (songRequestType == SongRequestType.Regular)
                {
                    return new AddSongResult
                    {
                        AddRequestResult = AddRequestResult.PlaylistClosed
                    };
                }

                break;
        }

        if (songRequestType == SongRequestType.SuperVip)
        {
            bool isSuperVipInQueue;
            using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
            {
                isSuperVipInQueue = repo.IsSuperVipInQueue();
            }

            if (isSuperVipInQueue)
                return new AddSongResult
                {
                    AddRequestResult = AddRequestResult.OnlyOneSuper
                };

            if (!await _vipService.UseSuperVip(username, 0).ConfigureAwait(false))
                return new AddSongResult
                {
                    AddRequestResult = AddRequestResult.NotEnoughVips
                };

            using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
            {
                return await repo.AddRequest(requestText, username, false, true, searchSongId);
            }
        }

        if (songRequestType == SongRequestType.Vip)
        {
            if (!await _vipService.UseVip(username))
                return new AddSongResult
                {
                    AddRequestResult = AddRequestResult.NotEnoughVips
                };

            using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
            {
                return await repo.AddRequest(requestText, username, true, false, searchSongId);
            }
        }

        if (songRequestType == SongRequestType.Regular)
        {
            var maxRegulars = _configService.Get<int>("MaxRegularSongsPerUser");

            int usersRegulars;
            using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
            {
                usersRegulars = repo.GetUsersCurrentRegularRequestCount(username);
            }

            if (usersRegulars >= maxRegulars)
                return new AddSongResult
                {
                    AddRequestResult = AddRequestResult.MaximumRegularRequests,
                    MaximumRegularRequests = maxRegulars
                };

            using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
            {
                return await repo.AddRequest(requestText, username, false, false, searchSongId);
            }
        }

        throw new Exception(
            $"Requested a new song of type Any, Username: {username}, RequestText: {requestText}");
    }

    private async Task<PromoteRequestIntermediate> Promote(string username, bool useSuperVip, int songRequestId = 0)
    {
        SongRequestIntermediate songRequest;
        using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
        {
            songRequest = await repo.GetRequest(songRequestId);
        }

        if (useSuperVip)
        {
            if (songRequest.IsVip)
            {
                if (!await _vipService.UseSuperVip(username, 1))
                {
                    return new PromoteRequestIntermediate
                    {
                        PlaylistIndex = 0,
                        PromoteRequestResult = PromoteRequestResult.NoVipAvailable
                    };
                }

                return new PromoteRequestIntermediate
                {
                    PlaylistIndex = 1,
                    PromoteRequestResult = PromoteRequestResult.Successful
                };
            }

            if (songRequest.IsSuperVip)
            {
                return new PromoteRequestIntermediate
                {
                    PromoteRequestResult = PromoteRequestResult.AlreadyVip,
                    PlaylistIndex = 0
                };
            }

            if (!await _vipService.UseSuperVip(username, 0))
            {
                return new PromoteRequestIntermediate
                {
                    PlaylistIndex = 0,
                    PromoteRequestResult = PromoteRequestResult.NoVipAvailable
                };
            }

            bool isSuperVipInQueue;
            using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
            {
                isSuperVipInQueue = repo.IsSuperVipInQueue();
            }

            if (isSuperVipInQueue)
            {
                return new PromoteRequestIntermediate
                {
                    PromoteRequestResult = PromoteRequestResult.AlreadyVip
                };
            }

            using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
            {
                await repo.PromoteUserRequest(username, songRequestId, true);
            }

            return new PromoteRequestIntermediate
            {
                PlaylistIndex = 1,
                PromoteRequestResult = PromoteRequestResult.Successful
            };
        }

        if (songRequest?.IsVip ?? false)
        {
            return new PromoteRequestIntermediate
            {
                PromoteRequestResult = PromoteRequestResult.AlreadyVip,
                PlaylistIndex = 0
            };
        }

        if (!await _vipService.UseVip(username))
            return new PromoteRequestIntermediate
            {
                PromoteRequestResult = PromoteRequestResult.NoVipAvailable
            };

        int newSongIndex;
        using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
        {
            newSongIndex = await repo.PromoteUserRequest(username, songRequestId);
        }

        return new PromoteRequestIntermediate
        {
            PromoteRequestResult =
                newSongIndex > 0 ? PromoteRequestResult.Successful : PromoteRequestResult.UnSuccessful,
            PlaylistIndex = newSongIndex
        };
    }

    private async Task ArchiveRequest(int requestId, bool refundVip)
    {
        string username;
        using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
        {
            username = await repo.ArchiveRequest(requestId);
        }

        if (refundVip)
        {
            SongRequestIntermediate songRequest;
            using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
            {
                await repo.ArchiveRequest(requestId);

                songRequest = await repo.GetRequest(requestId);
            }

            bool userInChat;
            using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
            {
                userInChat = await repo.IsUserInChat(songRequest.SongRequestUsername);
            }

            var request = new PlaylistItem
            {
                songRequestId = songRequest.SongRequestId,
                songRequestText = songRequest.SongRequestText,
                songRequester = songRequest.SongRequestUsername,
                isInChat = userInChat || songRequest.IsRecentRequest || songRequest.IsRecentVip ||
                           songRequest.IsRecentSuperVip,
                isVip = songRequest.IsVip,
                isSuperVip = songRequest.IsSuperVip,
                isInDrive = songRequest.IsInDrive
            };

            if (request.isVip || request.isSuperVip)
            {
                using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
                {
                    await repo.RefundVips(new List<VipRefund>
                    {
                        new VipRefund
                        {
                            Username = request.songRequester,
                            VipsToRefund = request.isVip ? 1 : _configService.Get<int>("SuperVipCost")
                        }
                    });
                }
            }
        }

        await _vipService.UpdateClientVips(username).ConfigureAwait(false);
    }

    private async Task<bool> ArchiveAndRefundVips(string username, SongRequestType songRequestType, int currentSongRequestId)
    {
        List<UsersRequestsIntermediate> usersRequests;
        using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
        {
            usersRequests = repo.GetUsersRequests(username);
        }

        int songRequestId = 0;
        switch (songRequestType)
        {
            case SongRequestType.Regular:
                songRequestId = usersRequests.SingleOrDefault(r => !r.IsVip)?.SongRequestId ?? 0;
                break;
            case SongRequestType.Vip:
                songRequestId = usersRequests.SingleOrDefault(r => r.IsVip && !r.IsSuperVip)?.SongRequestId ?? 0;
                break;
            case SongRequestType.SuperVip:
                songRequestId = usersRequests.SingleOrDefault(r => r.IsSuperVip)?.SongRequestId ?? 0;
                break;
            default:
                return false;
        }

        if (songRequestId == 0 || songRequestId == currentSongRequestId) return false;

        SongRequestIntermediate songRequest;
        using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
        {
            await repo.ArchiveRequest(songRequestId);

            songRequest = await repo.GetRequest(songRequestId);
        }

        bool userInChat;
        using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
        {
            userInChat = await repo.IsUserInChat(songRequest.SongRequestUsername);
        }

        var request = new PlaylistItem
        {
            songRequestId = songRequest.SongRequestId,
            songRequestText = songRequest.SongRequestText,
            songRequester = songRequest.SongRequestUsername,
            isInChat = userInChat || songRequest.IsRecentRequest || songRequest.IsRecentVip ||
                       songRequest.IsRecentSuperVip,
            isVip = songRequest.IsVip,
            isSuperVip = songRequest.IsSuperVip,
            isInDrive = songRequest.IsInDrive
        };

        if (request.isVip || request.isSuperVip)
        {
            using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
            {
                await repo.RefundVips(new List<VipRefund>
                {
                    new VipRefund
                    {
                        Username = request.songRequester,
                        VipsToRefund = request.isVip ? 1 : _configService.Get<int>("SuperVipCost")
                    }
                });
            }
        }

        await _vipService.UpdateClientVips(username).ConfigureAwait(false);

        return true;
    }

    private async Task<bool> RemoveByPlaylistIndex(string username, int playlistPosition)
    {
        List<UsersRequestsIntermediate> usersRequests;
        using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
        {
            usersRequests = repo.GetUsersRequests(username);
        }

        if (usersRequests == null || !usersRequests.Any()) return false;

        var request = usersRequests.SingleOrDefault(r => r.PlaylistPosition == playlistPosition);

        if (request == null) return false;

        SongRequestIntermediate songRequest;
        using (var repo = new SongRequestsRepository(_chatbotContextFactory, _configService))
        {
            await repo.ArchiveRequest(request.SongRequestId);

            songRequest = await repo.GetRequest(request.SongRequestId);
        }

        bool userInChat;
        using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
        {
            userInChat = await repo.IsUserInChat(songRequest.SongRequestUsername);
        }

        var playlistItem = new PlaylistItem
        {
            songRequestId = songRequest.SongRequestId,
            songRequestText = songRequest.SongRequestText,
            songRequester = songRequest.SongRequestUsername,
            isInChat = userInChat || songRequest.IsRecentRequest || songRequest.IsRecentVip ||
                       songRequest.IsRecentSuperVip,
            isVip = songRequest.IsVip,
            isSuperVip = songRequest.IsSuperVip,
            isInDrive = songRequest.IsInDrive
        };

        if (playlistItem.isVip || playlistItem.isSuperVip)
        {
            using (var repo = new UsersRepository(_chatbotContextFactory, _configService, _logger))
            {
                await repo.RefundVips(new List<VipRefund>
                {
                    new VipRefund
                    {
                        Username = playlistItem.songRequester,
                        VipsToRefund = playlistItem.isVip ? 1 : _configService.Get<int>("SuperVipCost")
                    }
                });
            }
        }

        await _vipService.UpdateClientVips(username).ConfigureAwait(false);

        return true;
    }
}