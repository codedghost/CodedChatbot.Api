using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Search;

public class SaveSearchSynonymRequestRepository : BaseRepository<SearchSynonymRequest>
{
    public SaveSearchSynonymRequestRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public async Task Save(string synonymRequest, string username)
    {
        var request = new SearchSynonymRequest
        {
            SynonymRequest = synonymRequest,
            Username = username
        };

        await CreateAndSaveAsync(request);
    }
}