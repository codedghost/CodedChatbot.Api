using System.Collections.Generic;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote
{
    public interface IGetQuotesRepository
    {
        public List<Database.Context.Models.Quote> Get();
    }
}