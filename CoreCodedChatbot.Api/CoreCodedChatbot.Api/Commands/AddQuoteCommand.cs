using CoreCodedChatbot.Api.Interfaces.Commands;
using CoreCodedChatbot.Api.Interfaces.Repositories;
using CoreCodedChatbot.Api.Repositories;

namespace CoreCodedChatbot.Api.Commands
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