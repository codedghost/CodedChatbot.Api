namespace CoreCodedChatbot.Api.Interfaces.Repositories
{
    public interface IAddQuoteRepository
    {
        int AddQuote(string username, string quoteText);
    }
}