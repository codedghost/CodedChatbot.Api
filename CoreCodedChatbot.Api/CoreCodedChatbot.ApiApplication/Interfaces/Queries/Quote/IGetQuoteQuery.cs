using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.Quote
{
    public interface IGetQuoteQuery
    {
        QuoteIntermediate GetQuote(int quoteId);
    }
}