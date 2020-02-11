using System.Collections.Generic;
using System.Linq;
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

        private PlaylistItem _currentRequest;

        public PlaylistService(
            IGetSongRequestByIdQuery getSongRequestByIdQuery,
            IGetPlaylistStateQuery getPlaylistStateQuery,
            IAddSongRequestCommand addSongRequestCommand,
            IPromoteUsersRegularRequestCommand promoteUsersRegularRequestCommand,
            IArchiveRequestCommand archiveRequestCommand,
            IRemoveAndRefundAllRequestsCommand removeAndRefundAllRequestsCommand,
            IGetCurrentRequestsQuery getCurrentRequestsQuery,
            IIsSuperVipInQueueQuery isSuperVipInQueueQuery
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

            var vipRequests = currentRequests.SongRequests.Where(sr => sr.IsVip || sr.IsSuperVip)
                .OrderBy(sr => sr.S)
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
        }

        public bool RemoveRockRequests(string username, string commandText, bool isMod)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateFullPlaylist(bool updateCurrent = false)
        {
            throw new System.NotImplementedException();
        }

        public string GetUserRequests(string username)
        {
            throw new System.NotImplementedException();
        }

        public List<string> GetUserRelevantRequests(string username)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }

        public bool OpenPlaylist()
        {
            throw new System.NotImplementedException();
        }

        public bool ClosePlaylist()
        {
            throw new System.NotImplementedException();
        }

        public bool VeryClosePlaylist()
        {
            throw new System.NotImplementedException();
        }

        public int GetMaxUserRequests()
        {
            throw new System.NotImplementedException();
        }

        public bool IsSuperRequestInQueue()
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

        public bool IsSuperVipRequestInQueue()
        {
            var isInQueue = _isSuperVipInQueueQuery.IsSuperVipInQueue();

            return isInQueue;
        }
    }
}