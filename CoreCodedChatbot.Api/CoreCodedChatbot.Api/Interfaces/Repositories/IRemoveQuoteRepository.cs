namespace CoreCodedChatbot.Api.Interfaces.Repositories
{
    public interface IRemoveQuoteRepository
    {
        void RemoveQuote(int quoteId, string username, bool isMod);
    }
}