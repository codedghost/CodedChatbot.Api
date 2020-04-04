namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Quote
{
    public interface IEditQuoteCommand
    {
        void EditQuote(int quoteId, string quoteText, string username, bool isMod);
    }
}