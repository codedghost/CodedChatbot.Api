using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiContract.ResponseModels.Quotes.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services
{
    public interface IQuoteService
    {
        Task<int> AddQuote(string username, string quoteText);
        Task EditQuote(int quoteId, string quoteText, string username, bool editRequestIsMod);
        Task RemoveQuote(int quoteId, string username, bool isMod);
        Task<Quote> GetQuote(int? quoteId);
        Task<List<Quote>> GetQuotes(int page, int pageSize);
    }
}