﻿using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Queries.Quote
{
    public class GetRandomQuoteQuery : IGetRandomQuoteQuery
    {
        private readonly IGetQuoteIdsRepository _getQuoteIdsRepository;
        private readonly IGetQuoteRepository _getQuoteRepository;
        private Random _quoteRand;

        public GetRandomQuoteQuery(
            IGetQuoteIdsRepository getQuoteIdsRepository,
            IGetQuoteRepository getQuoteRepository
            )
        {
            _getQuoteIdsRepository = getQuoteIdsRepository;
            _getQuoteRepository = getQuoteRepository;

            _quoteRand = new Random();
        }

        public QuoteIntermediate GetRandomQuote()
        {
            var ids = _getQuoteIdsRepository.GetQuoteIds();

            var random = _quoteRand.Next(0, ids.Count);

            var quoteId = ids[random];

            var quote = _getQuoteRepository.GetQuote(quoteId);

            return new QuoteIntermediate
            {
                QuoteId = quote.QuoteId,
                QuoteText = quote.QuoteText
            };
        }
    }
}