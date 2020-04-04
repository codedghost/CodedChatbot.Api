namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote
{
    public interface IGetQuoteRepository
    {
        Database.Context.Models.Quote GetQuote(int quoteId);
    }
}