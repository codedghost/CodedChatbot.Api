using CoreCodedChatbot.Api.Interfaces.Commands;
using CoreCodedChatbot.Api.Interfaces.Services;

namespace CoreCodedChatbot.Api.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly IAddQuoteCommand _addQuoteCommand;

        public QuoteService(
            IAddQuoteCommand addQuoteCommand
            )
        {
            _addQuoteCommand = addQuoteCommand;
        }

        public int AddQuote(string username, string quoteText)
        {
            var quoteId = _addQuoteCommand.AddQuote(username, quoteText);

            return quoteId;
        }
    }
}