using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Search;

public class SearchSynonymRequestsRepository : BaseRepository<SearchSynonymRequest>
{
    public SearchSynonymRequestsRepository(IChatbotContextFactory chatbotContextFactory) : base(chatbotContextFactory)
    {
    }
}