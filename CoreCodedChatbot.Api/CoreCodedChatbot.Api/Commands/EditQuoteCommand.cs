using CoreCodedChatbot.Api.Interfaces.Commands;
using CoreCodedChatbot.Api.Interfaces.Repositories;

namespace CoreCodedChatbot.Api.Commands
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