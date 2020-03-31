using CoreCodedChatbot.Api.Interfaces.Repositories;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.Api.Repositories
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