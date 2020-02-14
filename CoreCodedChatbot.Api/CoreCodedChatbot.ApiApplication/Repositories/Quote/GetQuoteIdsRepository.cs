using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Quote
{
    public class GetQuoteIdsRepository : IGetQuoteIdsRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetQuoteIdsRepository(
            IChatbotContextFactory chatbotContextFactory
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public List<int> GetQuoteIds()
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var quoteIds = context.Quotes.Select(q => q.QuoteId);

                return quoteIds.ToList();
            }
        }
    }
}