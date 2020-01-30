using CoreCodedChatbot.Api.Interfaces.Commands;
using CoreCodedChatbot.Api.Interfaces.Repositories;

namespace CoreCodedChatbot.Api.Commands
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