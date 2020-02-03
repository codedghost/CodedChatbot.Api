using CoreCodedChatbot.Api.Interfaces.Commands.Quote;
using CoreCodedChatbot.Api.Interfaces.Repositories.Quote;

namespace CoreCodedChatbot.Api.Commands.Quote
{
    public class EditQuoteCommand : IEditQuoteCommand
    {
        private readonly IEditQuoteRepository _editQuoteRepository;

        public EditQuoteCommand(
            IEditQuoteRepository editQuoteRepository
            )
        {
            _editQuoteRepository = editQuoteRepository;
        }

        public void EditQuote(int quoteId, string quoteText, string username, bool isMod)
        {
            _editQuoteRepository.EditQuote(quoteId, quoteText, username, isMod);
        }
    }
}