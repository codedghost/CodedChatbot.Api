namespace CoreCodedChatbot.Api.Interfaces.Commands
{
    public interface IRemoveQuoteCommand
    {
        void RemoveQuote(int quoteId, string username, bool isMod);
    }
}