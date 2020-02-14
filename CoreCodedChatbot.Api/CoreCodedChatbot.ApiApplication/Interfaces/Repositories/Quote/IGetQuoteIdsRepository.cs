using System.Collections.Generic;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote
{
    public interface IGetQuoteIdsRepository
    {
        List<int> GetQuoteIds();
    }
}