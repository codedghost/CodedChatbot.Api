using CoreCodedChatbot.Api.Interfaces.Commands;
using CoreCodedChatbot.Api.Interfaces.Services;

namespace CoreCodedChatbot.Api.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly IAddQuoteCommand _addQuoteCommand;
        private readonly IEditQuoteCommand _editQuoteCommand;

        public QuoteService(
            IAddQuoteCommand addQuoteCommand,
            IEditQuoteCommand editQuoteCommand
        )
        {
            _addQuoteCommand = addQuoteCommand;
            _editQuoteCommand = editQuoteCommand;
        }

        public int AddQuote(string username, string quoteText)
        {
            var quoteId = _addQuoteCommand.AddQuote(username, quoteText);

            return quoteId;
        }

        public void EditQuote(int quoteId, string quoteText, string username)
        {
            _editQuoteCommand.EditQuote(quoteId, quoteText, username);
        }
    }
}