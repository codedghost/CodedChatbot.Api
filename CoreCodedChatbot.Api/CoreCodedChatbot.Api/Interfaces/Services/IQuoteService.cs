namespace CoreCodedChatbot.Api.Interfaces.Services
{
    public interface IQuoteService
    {
        int AddQuote(string username, string quoteText);
    }
}