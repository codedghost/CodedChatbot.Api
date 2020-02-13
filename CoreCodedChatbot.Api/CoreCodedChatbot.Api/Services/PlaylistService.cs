using System;
using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.Api.Commands.Playlist;
using CoreCodedChatbot.Api.Interfaces.Commands.Playlist;
using CoreCodedChatbot.Api.Interfaces.Queries.Playlist;
using CoreCodedChatbot.Api.Interfaces.Services;
using CoreCodedChatbot.Api.Models.Enums;
using CoreCodedChatbot.ApiContract.Enums.Playlist;
using CoreCodedChatbot.ApiContract.RequestModels.Playlist;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist.ChildModels;
using CoreCodedChatbot.Library.Models.View;

namespace CoreCodedChatbot.Api.Services
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

        private PlaylistItem _currentRequest;
        private Random _rand;

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
            IAddSongToDriveCommand addSongToDriveCommand
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

            //TODO SignalR Update


            return (result.AddRequestResult, result.SongIndex);
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

            //TODO SignalR Update

            return result.AddRequestResult;
        }

        public AddRequestResult AddWebRequest(AddWebSongRequest requestSongViewModel, string username)
        {
            var requestText =
                $"{requestSongViewModel.Artist} - {requestSongViewModel.Title} - {requestSongViewModel.SelectedInstrument}";

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

            //TODO SignalR Update

            return result.AddRequestResult;
        }

        public PlaylistState GetPlaylistState()
        {
            var playlistState = _getPlaylistStateQuery.GetPlaylistState();

            return playlistState;
        }

        public int PromoteRequest(string username)
        {
            var result = _promoteUsersRegularRequestCommand.PromoteUsersRegularRequest(username);

            //TODO SignalR Update

            return result.PlaylistIndex;
        }

        public PromoteRequestResult PromoteWebRequest(int songId, string username)
        {
            var result = _promoteUsersRegularRequestCommand.PromoteUsersRegularRequest(username, songId);

            // TODO SignalR Update

            return result.PromoteRequestResult;
        }

        public void ArchiveCurrentRequest(int songId = 0)
        {
            var currentRequest = songId == 0 ? _currentRequest :
                songId == _currentRequest.songRequestId ? _currentRequest : null;

            if (currentRequest == null) return;

            _archiveRequestCommand.ArchiveRequest(currentRequest.songRequestId);

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

            // TODO SignalR Update

            return true;
        }

        public void ClearRockRequests()
        {
            _removeAndRefundAllRequestsCommand.RemoveAndRefundAllRequests();

            // TODO SignalR Update
        }

        public bool RemoveRockRequests(string username, string commandText, bool isMod)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public EditRequestResult EditWebRequest(RequestSongViewModel editRequestModel, string username, bool isMod)
        {
            throw new System.NotImplementedException();
        }

        public bool AddSongToDrive(int songId)
        {
            return _addSongToDriveCommand.AddSongToDrive(songId);
        }

        public bool OpenPlaylist()
        {
            return _updatePlaylistStateCommand.UpdatePlaylistState(PlaylistState.Open);
        }

        public bool ClosePlaylist()
        {
            return _updatePlaylistStateCommand.UpdatePlaylistState(PlaylistState.Closed);
        }

        public bool VeryClosePlaylist()
        {
            return _updatePlaylistStateCommand.UpdatePlaylistState(PlaylistState.VeryClosed);
        }

        public int GetMaxUserRequests()
        {
            throw new System.NotImplementedException();
        }

        public string EditSuperVipRequest(string username, string songText)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoveSuperRequest(string username)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateFullPlaylist(bool updateCurrent = false)
        {
            throw new System.NotImplementedException();
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
    }
}