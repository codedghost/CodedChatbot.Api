namespace CoreCodedChatbot.Api.Interfaces.Commands
{
    public interface IAddQuoteCommand
    {
        int AddQuote(string username, string quoteText);
    }
}