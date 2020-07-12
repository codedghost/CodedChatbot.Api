using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Queries.Quote
{
    public class GetQuotesQuery : IGetQuotesQuery
    {
        private readonly IGetQuotesRepository _getQuotesRepository;

        public GetQuotesQuery(
            IGetQuotesRepository getQuotesRepository
            )
        {
            _getQuotesRepository = getQuotesRepository;
        }

        public List<QuoteIntermediate> Get()
        {
            var quotes = _getQuotesRepository.Get();

            var intermediates = quotes.Select(q => new QuoteIntermediate
            {
                QuoteId = q.QuoteId,
                QuoteText = q.QuoteText,
                CreatedBy = q.CreatedBy
            });

            return intermediates.ToList();
        }
    }
}