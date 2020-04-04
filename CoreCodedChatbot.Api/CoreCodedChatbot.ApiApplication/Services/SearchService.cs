using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Search;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.RequestModels.Search;

namespace CoreCodedChatbot.ApiApplication.Services
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