using CoreCodedChatbot.Api.Interfaces.Commands.Playlist;
using CoreCodedChatbot.Api.Interfaces.Queries.Playlist;
using CoreCodedChatbot.Api.Interfaces.Queries.Vip;
using CoreCodedChatbot.Api.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.Api.Interfaces.Services;
using CoreCodedChatbot.Api.Models.Enums;
using CoreCodedChatbot.Api.Models.Intermediates;
using CoreCodedChatbot.ApiContract.Enums.Playlist;
using CoreCodedChatbot.Config;

namespace CoreCodedChatbot.Api.Commands.Playlist
{
    public class AddSongRequestCommand : IAddSongRequestCommand
    {
        private readonly IGetPlaylistStateQuery _getPlaylistStateQuery;
        private readonly ICheckUserHasMaxRegularsInQueueQuery _checkUserHasMaxRegularsInQueueQuery;
        private readonly IIsSuperVipInQueueQuery _isSuperVipInQueueQuery;
        private readonly ICheckUserHasVipsQuery _checkUserHasVipsQuery;
        private readonly IAddRequestRepository _addRequestRepository;
        private readonly IVipService _vipService;
        private readonly IConfigService _configService;

        public AddSongRequestCommand(
            IGetPlaylistStateQuery getPlaylistStateQuery,
            ICheckUserHasMaxRegularsInQueueQuery checkUserHasMaxRegularsInQueueQuery,
            IIsSuperVipInQueueQuery isSuperVipInQueueQuery,
            ICheckUserHasVipsQuery checkUserHasVipsQuery,
            IAddRequestRepository addRequestRepository,
            IVipService vipService,
            IConfigService configService
            )
        {
            _getPlaylistStateQuery = getPlaylistStateQuery;
            _checkUserHasMaxRegularsInQueueQuery = checkUserHasMaxRegularsInQueueQuery;
            _isSuperVipInQueueQuery = isSuperVipInQueueQuery;
            _checkUserHasVipsQuery = checkUserHasVipsQuery;
            _addRequestRepository = addRequestRepository;
            _vipService = vipService;
            _configService = configService;
        }

        public AddSongResult AddSongRequest(string username, string requestText, SongRequestType songRequestType)
        {
            if (string.IsNullOrWhiteSpace(requestText))
                return new AddSongResult
                {
                    AddRequestResult = AddRequestResult.NoRequestEntered
                };

            var playlistState = _getPlaylistStateQuery.GetPlaylistState();

            switch (playlistState)
            {
                case PlaylistState.VeryClosed:
                    return new AddSongResult
                    {
                        AddRequestResult = AddRequestResult.PlaylistVeryClosed
                    };
                case PlaylistState.Closed:
                    switch (songRequestType)
                    {
                        case SongRequestType.Regular:
                            return new AddSongResult
                            {
                                AddRequestResult = AddRequestResult.PlaylistClosed
                            };
                    }
                    break;
            }
            return ProcessAddingSongRequest(username, requestText, songRequestType);
        }

        private AddSongResult ProcessAddingSongRequest(string username, string requestText,
            SongRequestType songRequestType)
        {
            switch (songRequestType)
            {
                case SongRequestType.SuperVip:
                    return ProcessSuperVip(username, requestText);
                case SongRequestType.Vip:
                    return ProcessVip(username, requestText);
                default:
                    return ProcessRegular(username, requestText);
            }
        }

        private AddSongResult ProcessRegular(string username, string requestText)
        {
            var maxRegulars = _configService.Get<int>("MaxRegularSongsPerUser");

            if (_checkUserHasMaxRegularsInQueueQuery.UserHasMaxRegularsInQueue(username))
                return new AddSongResult
                {
                    AddRequestResult = AddRequestResult.MaximumRegularRequests,
                    MaximumRegularRequests = maxRegulars
                };

            return _addRequestRepository.AddRequest(requestText, username, false, false);
        }

        private AddSongResult ProcessVip(string username, string requestText)
        {
            if (!_vipService.UseVip(username))
                return new AddSongResult
                {
                    AddRequestResult = AddRequestResult.NotEnoughVips
                };

            return _addRequestRepository.AddRequest(requestText, username, true, false);
        }

        private AddSongResult ProcessSuperVip(string username, string requestText)
        {
            if (_isSuperVipInQueueQuery.IsSuperVipInQueue())
                return new AddSongResult
                {
                    AddRequestResult = AddRequestResult.OnlyOneSuper
                };

            if (!_vipService.UseSuperVip(username))
                return new AddSongResult
                {
                    AddRequestResult = AddRequestResult.NotEnoughVips
                };

            return _addRequestRepository.AddRequest(requestText, username, false, true);
        }
    }
}