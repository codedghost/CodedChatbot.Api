﻿using CoreCodedChatbot.Api.Models.Intermediates;

namespace CoreCodedChatbot.Api.Interfaces.Queries.Quote
{
    public interface IGetQuoteQuery
    {
        QuoteIntermediate GetQuote(int quoteId);
    }
}