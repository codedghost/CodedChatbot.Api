using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Enums;
using CoreCodedChatbot.ApiContract.Enums.Playlist;
using CoreCodedChatbot.ApiContract.RequestModels.Playlist;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist.ChildModels;
using CoreCodedChatbot.ApiContract.SignalRHubModels.API;
using CoreCodedChatbot.ApiContract.SignalRHubModels.Website;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Secrets;
using Microsoft.AspNetCore.SignalR.Client;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IGetSongRequestByIdQuery _getSongRequestByIdQuery;
        private readonly IGetPlaylistStateQuery _getPlaylistStateQuery;
        private readonly IAddSongRequestCommand _addSongRequestCommand;
        private readonly IPromoteRequestCommand _promoteRequestCommand;
        private readonly IArchiveRequestCommand _archiveRequestCommand;
        private readonly IRemoveAndRefundAllRequestsCommand _removeAndRefundAllRequestsCommand;
        private readonly IGetCurrentRequestsQuery _getCurrentRequestsQuery;
        private readonly IIsSuperVipInQueueQuery _isSuperVipInQueueQuery;
        private readonly IGetUsersFormattedRequestsQuery _getUsersFormattedRequestsQuery;
        private readonly IUpdatePlaylistStateCommand _updatePlaylistStateCommand;
        private readonly IAddSongToDriveCommand _addSongToDriveCommand;
        private readonly IGetMaxRegularRequestCountQuery _getMaxRegularRequestCountQuery;
        private readonly IEditSuperVipCommand _editSuperVipCommand;
        private readonly IRemoveSuperVipCommand _removeSuperVipCommand;
        private readonly IGetUsersCurrentRequestCountsQuery _getUsersCurrentRequestCountsQuery;
        private readonly IRemoveUsersRequestByPlaylistIndexCommand _removeUsersRequestByPlaylistIndexCommand;
        private readonly IArchiveUsersSingleRequestCommand _archiveUsersSingleRequestCommand;
        private readonly IGetSingleSongRequestIdRepository _getSingleSongRequestIdRepository;
        private readonly IGetUsersRequestAtPlaylistIndexQuery _getUsersRequestAtPlaylistIndexQuery;
        private readonly IEditRequestCommand _editRequestCommand;
        private readonly IGetTopTenRequestsQuery _getTopTenRequestsQuery;
        private readonly ISecretService _secretService;
        private readonly IConfigService _configService;
        private readonly ISignalRService _signalRService;
        private readonly IRefundVipCommand _refundVipCommand;

        private PlaylistItem _currentRequest;
        private Random _rand;

        private int _currentVipRequestsPlayed = 0;
        private int _concurrentVipSongsToPlay = 0;

        public PlaylistService(
            IGetSongRequestByIdQuery getSongRequestByIdQuery,
            IGetPlaylistStateQuery getPlaylistStateQuery,
            IAddSongRequestCommand addSongRequestCommand,
            IPromoteRequestCommand promoteRequestCommand,
            IArchiveRequestCommand archiveRequestCommand,
            IRemoveAndRefundAllRequestsCommand removeAndRefundAllRequestsCommand,
            IGetCurrentRequestsQuery getCurrentRequestsQuery,
            IIsSuperVipInQueueQuery isSuperVipInQueueQuery,
            IGetUsersFormattedRequestsQuery getUsersFormattedRequestsQuery,
            IUpdatePlaylistStateCommand updatePlaylistStateCommand,
            IAddSongToDriveCommand addSongToDriveCommand,
            IGetMaxRegularRequestCountQuery getMaxRegularRequestCountQuery,
            IEditSuperVipCommand editSuperVipCommand,
            IRemoveSuperVipCommand removeSuperVipCommand,
            IGetUsersCurrentRequestCountsQuery getUsersCurrentRequestCountsQuery,
            IRemoveUsersRequestByPlaylistIndexCommand removeUsersRequestByPlaylistIndexCommand,
            IArchiveUsersSingleRequestCommand archiveUsersSingleRequestCommand,
            IGetSingleSongRequestIdRepository getSingleSongRequestIdRepository,
            IGetUsersRequestAtPlaylistIndexQuery getUsersRequestAtPlaylistIndexQuery,
            IEditRequestCommand editRequestCommand,
            IGetTopTenRequestsQuery getTopTenRequestsQuery,
            ISecretService secretService,
            IConfigService configService,
            ISignalRService signalRService,
            IRefundVipCommand refundVipCommand
        )
        {
            _getSongRequestByIdQuery = getSongRequestByIdQuery;
            _getPlaylistStateQuery = getPlaylistStateQuery;
            _addSongRequestCommand = addSongRequestCommand;
            _promoteRequestCommand = promoteRequestCommand;
            _archiveRequestCommand = archiveRequestCommand;
            _removeAndRefundAllRequestsCommand = removeAndRefundAllRequestsCommand;
            _getCurrentRequestsQuery = getCurrentRequestsQuery;
            _isSuperVipInQueueQuery = isSuperVipInQueueQuery;
            _getUsersFormattedRequestsQuery = getUsersFormattedRequestsQuery;
            _updatePlaylistStateCommand = updatePlaylistStateCommand;
            _addSongToDriveCommand = addSongToDriveCommand;
            _getMaxRegularRequestCountQuery = getMaxRegularRequestCountQuery;
            _editSuperVipCommand = editSuperVipCommand;
            _removeSuperVipCommand = removeSuperVipCommand;
            _getUsersCurrentRequestCountsQuery = getUsersCurrentRequestCountsQuery;
            _removeUsersRequestByPlaylistIndexCommand = removeUsersRequestByPlaylistIndexCommand;
            _archiveUsersSingleRequestCommand = archiveUsersSingleRequestCommand;
            _getSingleSongRequestIdRepository = getSingleSongRequestIdRepository;
            _getUsersRequestAtPlaylistIndexQuery = getUsersRequestAtPlaylistIndexQuery;
            _editRequestCommand = editRequestCommand;
            _getTopTenRequestsQuery = getTopTenRequestsQuery;
            _secretService = secretService;
            _configService = configService;
            _signalRService = signalRService;
            _refundVipCommand = refundVipCommand;

            _concurrentVipSongsToPlay = configService.Get<int>("ConcurrentVipSongsToPlay");

            _rand = new Random();
        }

        public PlaylistItem GetRequestById(int songId)
        {
            var playlistItem = _getSongRequestByIdQuery.GetSongRequestById(songId);

            return playlistItem;
        }

        public async Task<(AddRequestResult, int)> AddRequest(string username, string commandText, bool vipRequest = false)
        {
            //TODO
            var result = await _addSongRequestCommand.AddSongRequest(username, commandText,
                vipRequest ? SongRequestType.Vip : SongRequestType.Regular).ConfigureAwait(false);

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

            //TODO
            var result = await _addSongRequestCommand.AddSongRequest(username, requestText,
                requestSongViewModel.IsSuperVip ? SongRequestType.SuperVip :
                requestSongViewModel.IsVip ? SongRequestType.Vip : SongRequestType.Regular).ConfigureAwait(false);


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
            var playlistState = _getPlaylistStateQuery.GetPlaylistState();

            return playlistState;
        }

        public async Task<PromoteSongResponse> PromoteRequest(string username, int songId, bool useSuperVip)
        {
            var result = await _promoteRequestCommand.Promote(username, useSuperVip, songId)
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
            var result = await _promoteRequestCommand.Promote(username, false, songId).ConfigureAwait(false);

            UpdateFullPlaylist();

            return result.PromoteRequestResult;
        }

        public void ArchiveCurrentRequest(int songId = 0)
        {
            var currentRequest = songId == 0 ? _currentRequest :
                songId == _currentRequest.songRequestId ? _currentRequest : null;

            if (currentRequest == null) return;

            _archiveRequestCommand.ArchiveRequest(currentRequest.songRequestId, false);

            UpdateFullPlaylist(true);
        }

        public GetAllSongsResponse GetAllSongs()
        {
            //TODO
            var currentRequests = _getCurrentRequestsQuery.GetCurrentRequests();

            var regularRequests = currentRequests.RegularRequests.ToArray();
            var vipRequests = currentRequests.VipRequests.ToArray();

            // Ensure if the playlist is populated then a request is made current
            if (_currentRequest == null)
            {
                if (currentRequests.VipRequests.Any())
                {
                    _currentRequest = currentRequests.VipRequests.First();

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
            await _archiveRequestCommand.ArchiveRequest(songId, true).ConfigureAwait(false);

            UpdateFullPlaylist();

            return true;
        }

        public async Task ClearRockRequests()
        {
            await _removeAndRefundAllRequestsCommand.RemoveAndRefundAllRequests();
            
            UpdateFullPlaylist();
        }

        public async Task<bool> RemoveRockRequests(string username, string commandText, bool isMod)
        {
            bool success = false;

            // If the command text doesn't parse, we should attempt to remove a regular request
            if (!int.TryParse(commandText.Trim(), out var playlistIndex))
            {
                // remove regular request if it exists
                if (_getUsersCurrentRequestCountsQuery.GetUsersCurrentRequestCounts(username,
                    SongRequestType.Regular) == 1)
                {
                    success = await _archiveUsersSingleRequestCommand.ArchiveAndRefundVips(username, SongRequestType.Regular, _currentRequest.songRequestId).ConfigureAwait(false);
                }

                // if true return, otherwise attempt to remove and refund a single vip
                else if (_getUsersCurrentRequestCountsQuery.GetUsersCurrentRequestCounts(username, SongRequestType.Vip) == 1)
                {
                    success = await _archiveUsersSingleRequestCommand.ArchiveAndRefundVips(username, SongRequestType.Vip, _currentRequest.songRequestId).ConfigureAwait(false);
                }

                else if (_getUsersCurrentRequestCountsQuery.GetUsersCurrentRequestCounts(username,
                    SongRequestType.SuperVip) == 1)
                {
                    success = await _archiveUsersSingleRequestCommand.ArchiveAndRefundVips(username, SongRequestType.SuperVip, _currentRequest.songRequestId).ConfigureAwait(false);
                }

                return success;
            }

            // Remove VIP request using provided playlistIndex.
            // Query can use existing GetUsersRequestsRepository to get the song request id
            success = await _removeUsersRequestByPlaylistIndexCommand.Remove(username, playlistIndex).ConfigureAwait(false);

            UpdateFullPlaylist();

            return success;
        }

        public string GetUserRequests(string username)
        {
            //TODO
            var formattedRequests = _getUsersFormattedRequestsQuery.GetUsersFormattedRequests(username);

            var requestString = formattedRequests.Any()
                ? string.Join(", ", formattedRequests)
                : "Looks like you don't have any requests right now. Get some in there! !howtorequest";

            return requestString;
        }

        public bool EditRequest(string username, string commandText, bool isMod, out string songRequestText, out bool syntaxError)
        {
            if (string.IsNullOrWhiteSpace(commandText))
            {
                songRequestText = string.Empty;
                syntaxError = true;
                return false;
            }

            var commandTextTerms = commandText.Trim().Split(" ");

            SongRequestType editRequestType;
            EditRequestResult result;

            // If the command text doesn't parse, we should attempt to edit a regular request
            if (!int.TryParse(commandTextTerms[0].Trim(), out var playlistIndex))
            {
                songRequestText = commandText;

                // edit regular request if it exists
                if (_getUsersCurrentRequestCountsQuery.GetUsersCurrentRequestCounts(username,
                    SongRequestType.Regular) == 1)
                {
                    editRequestType = SongRequestType.Regular;
                }
                // if true return, otherwise attempt to edit and refund a single vip
                else if (_getUsersCurrentRequestCountsQuery.GetUsersCurrentRequestCounts(username, SongRequestType.Vip) == 1)
                {
                    editRequestType = SongRequestType.Vip;
                }

                else if (_getUsersCurrentRequestCountsQuery.GetUsersCurrentRequestCounts(username,
                    SongRequestType.SuperVip) == 1)
                {
                    editRequestType = SongRequestType.SuperVip;
                }
                else
                {
                    songRequestText = string.Empty;
                    syntaxError = true;
                    return false;
                }

                var formattedRequest =
                    FormattedRequest.GetFormattedRequest(songRequestText);

                var songRequestId = _getSingleSongRequestIdRepository.Get(username, editRequestType);
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

                result = EditWebRequest(editRequestModel);
            }
            else
            {
                songRequestText = string.Join(' ', commandTextTerms);

                // Edit request at position playlistIndex
                var songRequest = _getUsersRequestAtPlaylistIndexQuery.Get(username, playlistIndex,
                    _currentRequest.isSuperVip || _currentRequest.isVip);

                if (songRequest == null)
                {
                    syntaxError = true;
                    songRequestText = string.Empty;
                    return false;
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

                result = EditWebRequest(editRequestModel);
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

            return result == EditRequestResult.Success;
        }

        public EditRequestResult EditWebRequest(EditWebRequestRequestModel editWebRequestRequestModel)
        {
            if (editWebRequestRequestModel == null) return EditRequestResult.NoRequestEntered;

            try
            {
                //TODO
                _editRequestCommand.Edit(editWebRequestRequestModel);

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

        public bool AddSongToDrive(int songId)
        {
            var result = _addSongToDriveCommand.AddSongToDrive(songId);

            UpdateFullPlaylist();

            return result;
        }

        public async Task<bool> OpenPlaylist()
        {
            var result = _updatePlaylistStateCommand.UpdatePlaylistState(PlaylistState.Open);

            if (result)
                await _signalRService.UpdatePlaylistState(PlaylistState.Open).ConfigureAwait(false);

            return result;
        }

        public async Task<bool> ClosePlaylist()
        {
            var updatePlaylistState = _updatePlaylistStateCommand.UpdatePlaylistState(PlaylistState.Closed);

            if (updatePlaylistState)
                await _signalRService.UpdatePlaylistState(PlaylistState.Closed).ConfigureAwait(false);

            return updatePlaylistState;
        }

        public async Task<bool> VeryClosePlaylist()
        {
            var updatePlaylistState = _updatePlaylistStateCommand.UpdatePlaylistState(PlaylistState.VeryClosed);

            if (updatePlaylistState)
                await _signalRService.UpdatePlaylistState(PlaylistState.VeryClosed).ConfigureAwait(false);

            return updatePlaylistState;
        }

        public int GetMaxUserRequests()
        {
            var result = _getMaxRegularRequestCountQuery.Get();

            return result;
        }

        public async Task<AddRequestResult> AddSuperVipRequest(string username, string commandText)
        {
            //TODO
            var result = await _addSongRequestCommand.AddSongRequest(username, commandText, SongRequestType.SuperVip);

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

        public bool EditSuperVipRequest(string username, string songText)
        {
            //TODO - Run through web edit
            var songId = _editSuperVipCommand.Edit(username, songText);

            UpdateFullPlaylist();

            return songId > 0;
        }

        public async Task<bool> RemoveSuperRequest(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;

            await _removeSuperVipCommand.Remove(username).ConfigureAwait(false);

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
            var current = requests.RegularList.SingleOrDefault(r => r.songRequestId == _currentRequest.songRequestId);
            _currentRequest = current ?? requests.VipList.SingleOrDefault(r => r.songRequestId == _currentRequest.songRequestId);

            requests.RegularList = requests.RegularList.Where(r => r.songRequestId != _currentRequest.songRequestId)
                .ToArray();
            requests.VipList = requests.VipList.Where(r => r.songRequestId != _currentRequest.songRequestId).ToArray();

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
            var isInQueue = _isSuperVipInQueueQuery.IsSuperVipInQueue();

            return isInQueue;
        }

        public PlaylistItem GetCurrentSongRequest()
        {
            return _currentRequest;
        }

        public List<PlaylistItem> GetTopTenPlaylistItems()
        {
            return _getTopTenRequestsQuery.Get();
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
    }
}