using System.Collections.Generic;

namespace CoreCodedChatbot.Api.Interfaces.Repositories
{
    public interface IGetQuoteIdsRepository
    {
        List<int> GetQuoteIds();
    }
}