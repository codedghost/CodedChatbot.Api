using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Moderation;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Moderation;

namespace CoreCodedChatbot.ApiApplication.Commands.Moderation
{
    public class TransferUserAccountCommand : ITransferUserAccountCommand
    {
        private readonly ITransferUserAccountRepository _transferUserAccountRepository;

        public TransferUserAccountCommand(
            ITransferUserAccountRepository transferUserAccountRepository
            )
        {
            _transferUserAccountRepository = transferUserAccountRepository;
        }

        public void Transfer(string moderatorUsername, string oldUsername, string newUsername)
        {
            _transferUserAccountRepository.Transfer(moderatorUsername, oldUsername, newUsername);
        }
    }
}