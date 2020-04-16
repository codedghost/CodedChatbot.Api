using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Search;
using CoreCodedChatbot.ApiContract.ResponseModels.Search.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Queries.Search
{
    public class GetSongBySearchIdQuery : IGetSongBySearchIdQuery
    {
        private readonly IGetSongBySearchIdRepository _getSongBySearchIdRepository;

        public GetSongBySearchIdQuery(IGetSongBySearchIdRepository getSongBySearchIdRepository)
        {
            _getSongBySearchIdRepository = getSongBySearchIdRepository;
        }

        public BasicSongSearchResult Get(int songId)
        {
            var songRequest = _getSongBySearchIdRepository.Get(songId);

            return songRequest;
        }
    }
}