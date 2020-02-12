using System;
using CoreCodedChatbot.Api.Interfaces.Queries.Playlist;
using CoreCodedChatbot.Api.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.Api.Interfaces.Repositories.Settings;
using CoreCodedChatbot.ApiContract.Enums.Playlist;

namespace CoreCodedChatbot.Api.Queries.Playlist
{
    public class GetPlaylistStateQuery : IGetPlaylistStateQuery
    {
        private readonly IGetSettingRepository _getSettingRepository;

        public GetPlaylistStateQuery(
            IGetSettingRepository getSettingRepository
            )
        {
            _getSettingRepository = getSettingRepository;
        }

        public PlaylistState GetPlaylistState()
        {
            var state = _getSettingRepository.Get<string>("PlaylistStatus");

            return string.IsNullOrWhiteSpace(state) ? PlaylistState.VeryClosed : Enum.Parse<PlaylistState>(state);
        }
    }
}