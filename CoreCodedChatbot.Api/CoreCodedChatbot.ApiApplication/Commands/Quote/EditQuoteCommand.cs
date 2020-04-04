using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote;

namespace CoreCodedChatbot.ApiApplication.Commands.Quote
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