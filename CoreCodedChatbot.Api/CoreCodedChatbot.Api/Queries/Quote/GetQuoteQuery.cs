using System;
using CoreCodedChatbot.Api.Interfaces.Queries.Quote;
using CoreCodedChatbot.Api.Interfaces.Repositories.Quote;
using CoreCodedChatbot.Api.Models.Intermediates;

namespace CoreCodedChatbot.Api.Queries.Quote
{
    public class GetQuoteQuery : IGetQuoteQuery
    {
        private readonly IGetQuoteRepository _getQuoteRepository;

        public GetQuoteQuery(
            IGetQuoteRepository getQuoteRepository
            )
        {
            _getQuoteRepository = getQuoteRepository;
        }

        public QuoteIntermediate GetQuote(int quoteId)
        {
            var quote = _getQuoteRepository.GetQuote(quoteId);

            if (!quote.Enabled) throw new UnauthorizedAccessException("Quote has been removed and should not be accessed");

            return new QuoteIntermediate
            {
                QuoteId = quote.QuoteId,
                QuoteText = quote.QuoteText
            };
        }
    }
}