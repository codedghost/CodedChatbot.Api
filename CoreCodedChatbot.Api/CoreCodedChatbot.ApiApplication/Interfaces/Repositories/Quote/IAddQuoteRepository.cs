namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote
{
    public interface IAddQuoteRepository
    {
        int AddQuote(string username, string quoteText);
    }
}