﻿using System;
using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Enums;
using CoreCodedChatbot.ApiContract.Enums.Playlist;
using CoreCodedChatbot.ApiContract.RequestModels.Playlist;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist.ChildModels;
using CoreCodedChatbot.ApiContract.SignalRHubModels;
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
        private readonly IPromoteUsersRegularRequestCommand _promoteUsersRegularRequestCommand;
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

        private PlaylistItem _currentRequest;
        private Random _rand;

        private int _currentVipRequestsPlayed = 0;
        private int _concurrentVipSongsToPlay = 0;

        public PlaylistService(
            IGetSongRequestByIdQuery getSongRequestByIdQuery,
            IGetPlaylistStateQuery getPlaylistStateQuery,
            IAddSongRequestCommand addSongRequestCommand,
            IPromoteUsersRegularRequestCommand promoteUsersRegularRequestCommand,
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
            ISignalRService signalRService
        )
        {
            _getSongRequestByIdQuery = getSongRequestByIdQuery;
            _getPlaylistStateQuery = getPlaylistStateQuery;
            _addSongRequestCommand = addSongRequestCommand;
            _promoteUsersRegularRequestCommand = promoteUsersRegularRequestCommand;
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

            _concurrentVipSongsToPlay = configService.Get<int>("ConcurrentVipSongsToPlay");

            _rand = new Random();
        }

        public PlaylistItem GetRequestById(int songId)
        {
            var playlistItem = _getSongRequestByIdQuery.GetSongRequestById(songId);

            return playlistItem;
        }

        public (AddRequestResult, int) AddRequest(string username, string commandText, bool vipRequest = false)
        {
            var result = _addSongRequestCommand.AddSongRequest(username, commandText,
                vipRequest ? SongRequestType.Vip : SongRequestType.Regular);

            if (_currentRequest == null)
            {
                _currentRequest = new PlaylistItem
                {
                    songRequestId = result.SongRequestId,
                    songRequestText = commandText,
                    songRequester = username,
                    isEvenIndex = false,
                    isInChat = true,
                    isVip = vipRequest,
                    isSuperVip = false,
                    isInDrive = false
                };
            }

            UpdateFullPlaylist();
            //TODO SignalR Update


            return (result.AddRequestResult, result.SongIndex);
        }

        public AddRequestResult AddWebRequest(AddWebSongRequest requestSongViewModel, string username)
        {
            var requestText =
                $"{requestSongViewModel.Artist} - {requestSongViewModel.Title} ({requestSongViewModel.SelectedInstrument})";

            var result = _addSongRequestCommand.AddSongRequest(username, requestText,
                requestSongViewModel.IsSuperVip ? SongRequestType.SuperVip :
                requestSongViewModel.IsVip ? SongRequestType.Vip : SongRequestType.Regular);


            if (_currentRequest == null)
            {
                _currentRequest = new PlaylistItem
                {
                    songRequestId = result.SongRequestId,
                    songRequestText = requestText,
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
            //TODO SignalR Update

            return result.AddRequestResult;
        }

        public PlaylistState GetPlaylistState()
        {
            var playlistState = _getPlaylistStateQuery.GetPlaylistState();

            return playlistState;
        }

        public PromoteSongResponse PromoteRequest(string username, int songId)
        {
            var result = _promoteUsersRegularRequestCommand.PromoteUsersRegularRequest(username, songId);

            UpdateFullPlaylist();
            //TODO SignalR Update

            return new PromoteSongResponse
            {
                PlaylistIndex = result.PlaylistIndex,
                PromoteRequestResult = result.PromoteRequestResult
            };
        }

        public PromoteRequestResult PromoteWebRequest(int songId, string username)
        {
            var result = _promoteUsersRegularRequestCommand.PromoteUsersRegularRequest(username, songId);

            UpdateFullPlaylist();
            // TODO SignalR Update

            return result.PromoteRequestResult;
        }

        public void ArchiveCurrentRequest(int songId = 0)
        {
            var currentRequest = songId == 0 ? _currentRequest :
                songId == _currentRequest.songRequestId ? _currentRequest : null;

            if (currentRequest == null) return;

            _archiveRequestCommand.ArchiveRequest(currentRequest.songRequestId);

            UpdateFullPlaylist(true);
            // TODO SignalR Update
        }

        public GetAllSongsResponse GetAllSongs()
        {
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

        public bool ArchiveRequestById(int songId)
        {
            _archiveRequestCommand.ArchiveRequest(songId);

            UpdateFullPlaylist();
            // TODO SignalR Update

            return true;
        }

        public void ClearRockRequests()
        {
            _removeAndRefundAllRequestsCommand.RemoveAndRefundAllRequests();
            
            UpdateFullPlaylist();
            // TODO SignalR Update
        }

        public bool RemoveRockRequests(string username, string commandText, bool isMod)
        {
            bool success = false;

            // If the command text doesn't parse, we should attempt to remove a regular request
            if (!int.TryParse(commandText.Trim(), out var playlistIndex))
            {
                // remove regular request if it exists
                if (_getUsersCurrentRequestCountsQuery.GetUsersCurrentRequestCounts(username,
                    SongRequestType.Regular) == 1)
                {
                    success = _archiveUsersSingleRequestCommand.ArchiveAndRefundVips(username, SongRequestType.Regular, _currentRequest.songRequestId);
                }

                // if true return, otherwise attempt to remove and refund a single vip
                else if (_getUsersCurrentRequestCountsQuery.GetUsersCurrentRequestCounts(username, SongRequestType.Vip) == 1)
                {
                    success = _archiveUsersSingleRequestCommand.ArchiveAndRefundVips(username, SongRequestType.Vip, _currentRequest.songRequestId);
                }

                else if (_getUsersCurrentRequestCountsQuery.GetUsersCurrentRequestCounts(username,
                    SongRequestType.SuperVip) == 1)
                {
                    success = _archiveUsersSingleRequestCommand.ArchiveAndRefundVips(username, SongRequestType.SuperVip, _currentRequest.songRequestId);
                }

                // TODO SignalR Update

                return success;
            }

            // Remove VIP request using provided playlistIndex.
            // Query can use existing GetUsersRequestsRepository to get the song request id
            success = _removeUsersRequestByPlaylistIndexCommand.Remove(username, playlistIndex);

            UpdateFullPlaylist();
            // TODO SignalR Update

            return success;
        }

        public string GetUserRequests(string username)
        {
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
                _editRequestCommand.Edit(editWebRequestRequestModel);

                UpdateFullPlaylist();
                // TODO SignalR Update

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
            // TODO SignalR update

            return result;
        }

        public bool OpenPlaylist()
        {
            var result = _updatePlaylistStateCommand.UpdatePlaylistState(PlaylistState.Open);

            // TODO SignalR update

            return result;
        }

        public bool ClosePlaylist()
        {
            var updatePlaylistState = _updatePlaylistStateCommand.UpdatePlaylistState(PlaylistState.Closed);

            // TODO SignalR update

            return updatePlaylistState;
        }

        public bool VeryClosePlaylist()
        {
            var updatePlaylistState = _updatePlaylistStateCommand.UpdatePlaylistState(PlaylistState.VeryClosed);

            // TODO SignalR update

            return updatePlaylistState;
        }

        public int GetMaxUserRequests()
        {
            var result = _getMaxRegularRequestCountQuery.Get();

            return result;
        }

        public AddRequestResult AddSuperVipRequest(string username, string commandText)
        {
            var result = _addSongRequestCommand.AddSongRequest(username, commandText, SongRequestType.SuperVip);

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
            //TODO SignalR Update

            return result.AddRequestResult;
        }

        public bool EditSuperVipRequest(string username, string songText)
        {
            var songId = _editSuperVipCommand.Edit(username, songText);

            UpdateFullPlaylist();
            // TODO SignalR Update - Use returned SongId to target update (may need more info)

            return songId > 0;
        }

        public bool RemoveSuperRequest(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;

            _removeSuperVipCommand.Remove(username);

            UpdateFullPlaylist();
            // TODO SignalR Update

            return true;
        }

        public async void UpdateFullPlaylist(bool updateCurrent = false)
        {
            var psk = _secretService.GetSecret<string>("SignalRKey");

            var connection = _signalRService.GetCurrentConnection();

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