namespace CoreCodedChatbot.Api.Interfaces.Repositories.Quote
{
    public interface IEditQuoteRepository
    {
        void EditQuote(int quoteId, string quoteText, string username, bool isMod);
    }
}