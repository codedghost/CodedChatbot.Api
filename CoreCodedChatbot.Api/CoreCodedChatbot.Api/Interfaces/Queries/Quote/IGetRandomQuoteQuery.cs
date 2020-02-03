using CoreCodedChatbot.Api.Intermediates;

namespace CoreCodedChatbot.Api.Interfaces.Queries.Quote
{
    public interface IGetRandomQuoteQuery
    {
        QuoteIntermediate GetRandomQuote();
    }
}