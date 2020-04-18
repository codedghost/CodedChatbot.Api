using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Search;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Queries.Search
{
    public class GetSongBySearchIdQuery : IGetSongBySearchIdQuery
    {
        private readonly IGetSongBySearchIdRepository _getSongBySearchIdRepository;

        public GetSongBySearchIdQuery(IGetSongBySearchIdRepository getSongBySearchIdRepository)
        {
            _getSongBySearchIdRepository = getSongBySearchIdRepository;
        }

        public SongSearchIntermediate Get(int songId)
        {
            var songRequest = _getSongBySearchIdRepository.Get(songId);

            return songRequest;
        }
    }
}