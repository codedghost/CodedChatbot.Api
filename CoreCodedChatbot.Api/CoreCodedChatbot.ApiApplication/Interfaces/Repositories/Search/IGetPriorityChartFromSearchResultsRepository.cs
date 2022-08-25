using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Models.Solr;
using CoreCodedChatbot.ApiContract.ResponseModels.Search.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Search
{
    public interface IGetPriorityChartFromSearchResultsRepository
    {
        Task<BasicSongSearchResult> Get(List<SongSearch> solrResults);
    }
}