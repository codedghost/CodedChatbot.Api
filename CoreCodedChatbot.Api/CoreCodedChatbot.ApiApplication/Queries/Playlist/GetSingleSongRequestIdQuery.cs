using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Models.Enums;

namespace CoreCodedChatbot.ApiApplication.Queries.Playlist
{
    public class GetSingleSongRequestIdQuery : IGetSingleSongRequestIdQuery
    {
        private readonly IGetSingleSongRequestIdRepository _getSingleSongRequestIdRepository;

        public GetSingleSongRequestIdQuery(
            IGetSingleSongRequestIdRepository getSingleSongRequestIdRepository
            )
        {
            _getSingleSongRequestIdRepository = getSingleSongRequestIdRepository;
        }

        public int Get(string username, SongRequestType songRequestType)
        {
            return _getSingleSongRequestIdRepository.Get(username, songRequestType);
        }
    }
}