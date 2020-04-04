using CoreCodedChatbot.ApiContract.RequestModels.Search;

namespace CoreCodedChatbot.Api.Interfaces.Services
{
    public interface ISearchService
    {
        bool SaveSearchSynonymRequest(SaveSearchSynonymRequest request);
    }
}