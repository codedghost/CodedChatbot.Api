using CoreCodedChatbot.Api.Interfaces.Queries.Playlist;
using CoreCodedChatbot.Api.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiContract.Enums.Playlist;

namespace CoreCodedChatbot.Api.Queries.Playlist
{
    public class GetPlaylistStateQuery : IGetPlaylistStateQuery
    {
        private readonly IGetPlaylistStateRepository _getPlaylistStateRepository;

        public GetPlaylistStateQuery(
            IGetPlaylistStateRepository getPlaylistStateRepository
            )
        {
            _getPlaylistStateRepository = getPlaylistStateRepository;
        }

        public PlaylistState GetPlaylistState()
        {
            var state = _getPlaylistStateRepository.GetPlaylistState();

            return state;
        }
    }
}