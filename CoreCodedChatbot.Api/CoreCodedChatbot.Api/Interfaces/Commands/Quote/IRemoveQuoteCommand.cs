namespace CoreCodedChatbot.Api.Interfaces.Commands.Quote
{
    public interface IRemoveQuoteCommand
    {
        void RemoveQuote(int quoteId, string username, bool isMod);
    }
}