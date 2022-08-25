using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Search;
using CoreCodedChatbot.ApiApplication.Models.Solr;
using CoreCodedChatbot.ApiContract.ResponseModels.Search.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Queries.Search
{
    public class GetPriorityChartFromSearchResultsQuery : IGetPriorityChartFromSearchResultsQuery
    {
        private readonly IGetPriorityChartFromSearchResultsRepository _getPriorityChartFromSearchResultsRepository;

        public GetPriorityChartFromSearchResultsQuery(
            IGetPriorityChartFromSearchResultsRepository getPriorityChartFromSearchResultsRepository)
        {
            _getPriorityChartFromSearchResultsRepository = getPriorityChartFromSearchResultsRepository;
        }

        public async Task<BasicSongSearchResult> Get(List<SongSearch> solrResults, bool exactMatch)
        {
            return await _getPriorityChartFromSearchResultsRepository.Get(exactMatch
                ? solrResults
                : new List<SongSearch>
                {
                    solrResults.First()
                }).ConfigureAwait(false);
        }
    }
}