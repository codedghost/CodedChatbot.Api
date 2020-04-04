using CoreCodedChatbot.Api.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote;
using CoreCodedChatbot.ApiContract.RequestModels.Search;

namespace CoreCodedChatbot.Api.Services
{
    public class SearchService : ISearchService
    {
        private readonly ISaveSearchSynonymRequestCommand _saveSearchSynonymRequestCommand;

        public SearchService(ISaveSearchSynonymRequestCommand saveSearchSynonymRequestCommand)
        {
            _saveSearchSynonymRequestCommand = saveSearchSynonymRequestCommand;
        }

        public bool SaveSearchSynonymRequest(SaveSearchSynonymRequest request)
        {
            return _saveSearchSynonymRequestCommand.Save(request.SearchSynonymRequest, request.Username);
        }
    }
}