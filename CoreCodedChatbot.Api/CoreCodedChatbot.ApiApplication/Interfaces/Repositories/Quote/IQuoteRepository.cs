using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote;

public interface IQuoteRepository : IBaseRepository<Database.Context.Models.Quote>
{
    Task Archive(int quoteId, string username, bool isMod);
    Task Edit(int quoteId, string quoteText, string username, bool isMod);
}