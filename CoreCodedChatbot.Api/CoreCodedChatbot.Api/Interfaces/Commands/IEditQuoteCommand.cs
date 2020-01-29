namespace CoreCodedChatbot.Api.Interfaces.Commands
{
    public interface IEditQuoteCommand
    {
        void EditQuote(int quoteId, string quoteText, string username);
    }
}