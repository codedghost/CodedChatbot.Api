using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.Quote
{
    public interface IGetQuotesQuery
    {
        List<QuoteIntermediate> Get();
    }
}