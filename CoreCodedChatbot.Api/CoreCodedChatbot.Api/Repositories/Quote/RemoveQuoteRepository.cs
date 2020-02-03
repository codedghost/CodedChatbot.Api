using System;
using System.Collections.Generic;
using CoreCodedChatbot.Api.Interfaces.Repositories.Quote;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.Api.Repositories.Quote
{
    public class RemoveQuoteRepository : IRemoveQuoteRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public RemoveQuoteRepository(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void RemoveQuote(int quoteId, string username, bool isMod)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var quote = context.Quotes.Find(quoteId);

                if (quote == null) throw new KeyNotFoundException($"Could not retrieve a Quote with ID: {quoteId}");

                if (!isMod && string.Equals(quote.CreatedBy, username, StringComparison.InvariantCultureIgnoreCase))
                    throw new UnauthorizedAccessException(
                        $"This user cannot edit this quote as it does not belong to them. Owner: {quote.CreatedBy}, Editor: {username}");

                quote.Enabled = false;
                quote.LastEditedBy = username;
                quote.LastEdited = DateTime.Now;

                context.SaveChanges();
            }
        }
    }
}