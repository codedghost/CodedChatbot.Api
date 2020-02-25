using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Models.Enums;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiContract.Enums.Playlist;

namespace CoreCodedChatbot.ApiApplication.Commands.Playlist
{
    public class AddSongRequestCommand : IAddSongRequestCommand
    {
        private readonly IGetPlaylistStateQuery _getPlaylistStateQuery;
        private readonly IProcessSongRequestCommand _processSongRequestCommand;

        public AddSongRequestCommand(
            IGetPlaylistStateQuery getPlaylistStateQuery,
            IProcessSongRequestCommand processSongRequestCommand
            )
        {
            _getPlaylistStateQuery = getPlaylistStateQuery;
            _processSongRequestCommand = processSongRequestCommand;
        }

        public AddSongResult AddSongRequest(string username, string requestText, SongRequestType songRequestType)
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

            var playlistState = _getPlaylistStateQuery.GetPlaylistState();

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
            return _processSongRequestCommand.ProcessAddingSongRequest(username, requestText, songRequestType);
        }
    }
}