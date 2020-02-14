using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Settings;
using CoreCodedChatbot.ApiContract.Enums.Playlist;

namespace CoreCodedChatbot.ApiApplication.Queries.Playlist
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