using CoreCodedChatbot.Api.Interfaces.Repositories;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.Api.Repositories
{
    public class GetQuoteRepository : IGetQuoteRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetQuoteRepository(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public Quote GetQuote(int quoteId)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var quote = context.Quotes.Find(quoteId);

                return quote;
            }
        }
    }
}