namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Quote
{
    public interface IAddQuoteCommand
    {
        int AddQuote(string username, string quoteText);
    }
}