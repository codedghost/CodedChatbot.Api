using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote;

namespace CoreCodedChatbot.ApiApplication.Commands.Quote
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