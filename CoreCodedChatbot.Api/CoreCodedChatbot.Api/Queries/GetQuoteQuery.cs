using System;
using CoreCodedChatbot.Api.Interfaces.Queries;
using CoreCodedChatbot.Api.Interfaces.Repositories;
using CoreCodedChatbot.Api.Intermediates;

namespace CoreCodedChatbot.Api.Queries
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