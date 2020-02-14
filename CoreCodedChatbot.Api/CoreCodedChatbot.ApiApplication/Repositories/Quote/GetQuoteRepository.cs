using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Quote
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

        public Database.Context.Models.Quote GetQuote(int quoteId)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var quote = context.Quotes.Find(quoteId);

                return quote;
            }
        }
    }
}