﻿using CoreCodedChatbot.Api.Models.Intermediates;

namespace CoreCodedChatbot.Api.Interfaces.Services
{
    public interface IQuoteService
    {
        int AddQuote(string username, string quoteText);
        void EditQuote(int quoteId, string quoteText, string username, bool editRequestIsMod);
        void RemoveQuote(int quoteId, string username, bool isMod);
        QuoteIntermediate GetQuote(int? quoteId);
    }
}