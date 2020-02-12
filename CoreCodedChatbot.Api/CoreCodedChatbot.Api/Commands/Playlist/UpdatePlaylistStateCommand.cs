using CoreCodedChatbot.Api.Interfaces.Commands.Playlist;
using CoreCodedChatbot.Api.Interfaces.Repositories.Settings;
using CoreCodedChatbot.ApiContract.Enums.Playlist;

namespace CoreCodedChatbot.Api.Commands.Playlist
{
    public class UpdatePlaylistStateCommand : IUpdatePlaylistStateCommand
    {
        private readonly ISetOrCreateSettingRepository _setOrCreateSettingRepository;

        public UpdatePlaylistStateCommand(
            ISetOrCreateSettingRepository setOrCreateSettingRepository
            )
        {
            _setOrCreateSettingRepository = setOrCreateSettingRepository;
        }

        public bool UpdatePlaylistState(PlaylistState state)
        {
            switch (state)
            {
                case PlaylistState.VeryClosed:
                    _setOrCreateSettingRepository.Set("PlaylistStatus", "VeryClosed");
                    return true;
                case PlaylistState.Closed:
                    _setOrCreateSettingRepository.Set("PlaylistStatus", "Closed");
                    return true;
                case PlaylistState.Open:
                    _setOrCreateSettingRepository.Set("PlaylistStatus", "Open");
                    return true;
                default:
                    return false;
            }
        }
    }
}