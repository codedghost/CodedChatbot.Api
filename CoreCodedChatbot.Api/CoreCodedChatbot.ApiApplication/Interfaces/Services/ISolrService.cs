using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Models.Solr;
using CoreCodedChatbot.ApiContract.ResponseModels.Search.ChildModels;
using SolrNet;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services
{
    public interface ISolrService
    {
        Task<List<BasicSongSearchResult>> Search(string input);
        Task<List<BasicSongSearchResult>> SearchWithFallback(string artist, string songName);
        Task<BasicSongSearchResult> SearchSingleWithFallback(string artist, string songName);
    }
}