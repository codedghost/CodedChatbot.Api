using System.Threading.Tasks;
using CoreCodedChatbot.ApiContract.ResponseModels.Quotes;
using CoreCodedChatbot.ApiContract.ResponseModels.Quotes.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services;

public interface IQuoteService
{
    Task<int> AddQuote(string username, string quoteText);
    Task EditQuote(int quoteId, string quoteText, string username, bool editRequestIsMod);
    Task RemoveQuote(int quoteId, string username, bool isMod);
    Task<Quote> GetQuote(int? quoteId);
    Task<GetQuotesResponse> GetQuotes(int? page, int? pageSize, string? orderByColumnName, bool? desc, string? filterByColumn, object? filterValue);
    Task<bool> SendQuoteToChat(int id, string username);
}