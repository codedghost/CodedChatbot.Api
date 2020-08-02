
using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Quote
{
    public class GetQuotesRepository : IGetQuotesRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetQuotesRepository(IChatbotContextFactory chatbotContextFactory)
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public List<Database.Context.Models.Quote> Get()
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var quotes = context.Quotes.ToList();

                return quotes;
            }
        }
    }
}