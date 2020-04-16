using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiContract.ResponseModels.Search.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services
{
    public interface ISolrService
    {
        Task<List<BasicSongSearchResult>> Search(string input);
        Task<List<BasicSongSearchResult>> Search(string artist, string songName);
    }
}