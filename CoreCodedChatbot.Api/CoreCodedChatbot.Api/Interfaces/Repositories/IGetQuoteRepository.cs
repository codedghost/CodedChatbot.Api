using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.Api.Interfaces.Repositories
{
    public interface IGetQuoteRepository
    {
        Quote GetQuote(int quoteId);
    }
}