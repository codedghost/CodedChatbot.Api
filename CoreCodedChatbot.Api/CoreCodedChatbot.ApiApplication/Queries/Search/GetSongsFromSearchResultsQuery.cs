using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Search;
using CoreCodedChatbot.ApiApplication.Models.Solr;
using CoreCodedChatbot.ApiContract.ResponseModels.Search.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Queries.Search
{
    public class GetSongsFromSearchResultsQuery : IGetSongsFromSearchResultsQuery
    {
        private readonly IGetSongsFromSearchResultsRepository _getSongsFromSearchResultsRepository;

        public GetSongsFromSearchResultsQuery(
            IGetSongsFromSearchResultsRepository getSongsFromSearchResultsRepository
            )
        {
            _getSongsFromSearchResultsRepository = getSongsFromSearchResultsRepository;
        }

        public async Task<List<BasicSongSearchResult>> Get(List<SongSearch> searchResults)
        {
            return await _getSongsFromSearchResultsRepository.Get(searchResults).ConfigureAwait(false);
        }
    }
}