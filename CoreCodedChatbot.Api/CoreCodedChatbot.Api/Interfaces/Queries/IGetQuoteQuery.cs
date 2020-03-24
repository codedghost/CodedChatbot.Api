using CoreCodedChatbot.Api.Intermediates;

namespace CoreCodedChatbot.Api.Interfaces.Queries
{
    public interface IGetQuoteQuery
    {
        QuoteIntermediate GetQuote(int quoteId);
    }
}