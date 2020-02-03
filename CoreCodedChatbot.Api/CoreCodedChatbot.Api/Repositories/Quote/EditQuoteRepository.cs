using System;
using System.Collections.Generic;
using CoreCodedChatbot.Api.Interfaces.Repositories.Quote;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.Api.Repositories.Quote
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

        public void EditQuote(int quoteId, string quoteText, string username, bool isMod)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var quote = context.Quotes.Find(quoteId);

                if (quote == null) throw new KeyNotFoundException($"Quote not found with QuoteId: {quoteId}");

                if (!isMod && !string.Equals(quote.CreatedBy, username, StringComparison.InvariantCultureIgnoreCase))
                    throw new UnauthorizedAccessException(
                        $"This user cannot edit this quote as it does not belong to them. Owner: {quote.CreatedBy}, Editor: {username}");

                if (!quote.Enabled)
                    throw new UnauthorizedAccessException("This quote is disabled and therefore cannot be changed");

                quote.QuoteText = quoteText;

                quote.LastEdited = DateTime.Now;
                quote.LastEditedBy = username;

                context.SaveChanges();
            }
        }
    }
}