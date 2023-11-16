using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Models.Solr;
using CoreCodedChatbot.ApiContract.ResponseModels.Search.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.Search;

public interface IGetPriorityChartFromSearchResultsQuery
{
    Task<BasicSongSearchResult> Get(List<SongSearch> solrResults, bool exactMatch);
}