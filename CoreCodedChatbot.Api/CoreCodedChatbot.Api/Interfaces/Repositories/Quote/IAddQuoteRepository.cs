namespace CoreCodedChatbot.Api.Interfaces.Repositories.Quote
{
    public interface IAddQuoteRepository
    {
        int AddQuote(string username, string quoteText);
    }
}