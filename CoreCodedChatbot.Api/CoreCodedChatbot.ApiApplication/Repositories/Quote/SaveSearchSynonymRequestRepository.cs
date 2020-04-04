using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Search;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Quote
{
    public class SaveSearchSynonymRequestRepository : ISaveSearchSynonymRequestRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public SaveSearchSynonymRequestRepository(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void Save(string synonymRequest, string username)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var request = new SearchSynonymRequest
                {
                    SynonymRequest = synonymRequest,
                    Username = username
                };

                context.SearchSynonymRequests.Add(request);

                context.SaveChanges();
            }
        }
    }
}