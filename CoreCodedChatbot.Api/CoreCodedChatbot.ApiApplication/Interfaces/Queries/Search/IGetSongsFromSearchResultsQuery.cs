using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Models.Solr;
using CoreCodedChatbot.ApiContract.ResponseModels.Search.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.Search
{
    public interface IGetSongsFromSearchResultsQuery
    {
        List<BasicSongSearchResult> Get(List<SongSearch> searchResults);
    }
}