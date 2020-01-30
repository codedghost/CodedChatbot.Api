namespace CoreCodedChatbot.Api.Interfaces.Repositories
{
    public interface IEditQuoteRepository
    {
        void EditQuote(int quoteId, string quoteText, string username, bool isMod);
    }
}