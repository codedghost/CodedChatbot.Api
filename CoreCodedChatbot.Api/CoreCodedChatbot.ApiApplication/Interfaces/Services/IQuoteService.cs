using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services
{
    public interface IQuoteService
    {
        int AddQuote(string username, string quoteText);
        void EditQuote(int quoteId, string quoteText, string username, bool editRequestIsMod);
        void RemoveQuote(int quoteId, string username, bool isMod);
        QuoteIntermediate GetQuote(int? quoteId);
        List<QuoteIntermediate> GetQuotes();
    }
}