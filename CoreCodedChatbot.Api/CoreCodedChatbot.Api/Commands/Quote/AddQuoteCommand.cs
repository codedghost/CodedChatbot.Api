using CoreCodedChatbot.Api.Interfaces.Commands.Quote;
using CoreCodedChatbot.Api.Interfaces.Repositories.Quote;

namespace CoreCodedChatbot.Api.Commands.Quote
{
    public class AddQuoteCommand : IAddQuoteCommand
    {
        private readonly IAddQuoteRepository _addQuoteRepository;

        public AddQuoteCommand(
            IAddQuoteRepository addQuoteRepository
            )
        {
            _addQuoteRepository = addQuoteRepository;
        }

        public int AddQuote(string username, string quoteText)
        {
            return _addQuoteRepository.AddQuote(username, quoteText);
        }
    }
}