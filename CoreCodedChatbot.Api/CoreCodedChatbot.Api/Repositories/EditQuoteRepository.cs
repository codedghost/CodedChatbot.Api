using System;
using System.Collections.Generic;
using CoreCodedChatbot.Api.Interfaces.Repositories;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.Api.Repositories
{
    public class EditQuoteRepository : IEditQuoteRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public EditQuoteRepository(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void EditQuote(int quoteId, string quoteText, string username)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var quote = context.Quotes.Find(quoteId);

                if (quote == null) throw new KeyNotFoundException($"Quote not found with QuoteId: {quoteId}");

                quote.QuoteText = quoteText;

                quote.LastEdited = DateTime.Now;
                quote.LastEditedBy = username;

                context.SaveChanges();
            }
        }
    }
}