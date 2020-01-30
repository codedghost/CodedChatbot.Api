using CoreCodedChatbot.Api.Intermediates;

namespace CoreCodedChatbot.Api.Interfaces.Queries
{
    public interface IGetRandomQuoteQuery
    {
        QuoteIntermediate GetRandomQuote();
    }
}