using System.Collections.Generic;

namespace CoreCodedChatbot.Api.Interfaces.Repositories.Quote
{
    public interface IGetQuoteIdsRepository
    {
        List<int> GetQuoteIds();
    }
}