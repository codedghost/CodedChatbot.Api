using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.Quote
{
    public interface IGetRandomQuoteQuery
    {
        QuoteIntermediate GetRandomQuote();
    }
}