using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Models.Solr;
using CoreCodedChatbot.ApiContract.ResponseModels.Search.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.Search
{
    public interface IGetSongsFromSearchResultsQuery
    {
        Task<List<BasicSongSearchResult>> Get(List<SongSearch> searchResults);
    }
}