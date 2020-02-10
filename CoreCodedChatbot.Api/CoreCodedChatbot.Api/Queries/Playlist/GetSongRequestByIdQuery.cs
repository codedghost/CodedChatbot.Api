using CoreCodedChatbot.Api.Interfaces.Queries.Playlist;
using CoreCodedChatbot.Api.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist.ChildModels;

namespace CoreCodedChatbot.Api.Queries.Playlist
{
    public class GetSongRequestByIdQuery : IGetSongRequestByIdQuery
    {
        private readonly IGetSongRequestByIdRepository _getSongRequestByIdRepository;
        private readonly IGetIsUserInChatRepository _getIsUserInChatRepository;

        public GetSongRequestByIdQuery(
            IGetSongRequestByIdRepository getSongRequestByIdRepository,
            IGetIsUserInChatRepository getIsUserInChatRepository
            )
        {
            _getSongRequestByIdRepository = getSongRequestByIdRepository;
            _getIsUserInChatRepository = getIsUserInChatRepository;
        }

        public PlaylistItem GetSongRequestById(int id)
        {
            var songRequest = _getSongRequestByIdRepository.GetRequest(id);

            var userInChat = _getIsUserInChatRepository.IsUserInChat(songRequest.SongRequestUsername);

            return new PlaylistItem
            {
                songRequestId = songRequest.SongRequestId,
                songRequestText = songRequest.SongRequestText,
                songRequester = songRequest.SongRequestUsername,
                isInChat = userInChat || songRequest.IsRecentRequest || songRequest.IsRecentVip ||
                           songRequest.IsRecentSuperVip,
                isVip = songRequest.IsVip,
                isSuperVip = songRequest.IsSuperVip,
                isInDrive = songRequest.IsInDrive
            };
        }
    }
}