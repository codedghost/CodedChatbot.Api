using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Quote;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Quote;

namespace CoreCodedChatbot.ApiApplication.Commands.Quote
{
    public class RemoveQuoteCommand : IRemoveQuoteCommand
    {
        private readonly IRemoveQuoteRepository _removeQuoteRepository;

        public RemoveQuoteCommand(
            IRemoveQuoteRepository removeQuoteRepository
            )
        {
            _removeQuoteRepository = removeQuoteRepository;
        }

        public void RemoveQuote(int quoteId, string username, bool isMod)
        {
            _removeQuoteRepository.RemoveQuote(quoteId, username, isMod);
        }
    }
}