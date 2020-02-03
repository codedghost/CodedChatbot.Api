namespace CoreCodedChatbot.Api.Interfaces.Repositories.Quote
{
    public interface IRemoveQuoteRepository
    {
        void RemoveQuote(int quoteId, string username, bool isMod);
    }
}