using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Quote
{
    public class AddQuoteRepository : IAddQuoteRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public AddQuoteRepository(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public int AddQuote(string username, string quoteText)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var quote = new Database.Context.Models.Quote
                {
                    QuoteText = quoteText,
                    LastEdited = DateTime.Now,
                    CreatedBy = username,
                    Enabled = true
                };

                context.Quotes.Add(quote);
                context.SaveChanges();

                return quote.QuoteId;
            }
        }
    }
}