using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Moderation;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public class ModerationService : IModerationService
    {
        private readonly ITransferUserAccountCommand _transferUserAccountCommand;

        public ModerationService(ITransferUserAccountCommand transferUserAccountCommand)
        {
            _transferUserAccountCommand = transferUserAccountCommand;
        }

        public void TransferUserAccount(string moderationUsername, string oldUsername, string newUsername)
        {
            _transferUserAccountCommand.Transfer(moderationUsername, oldUsername, newUsername);
        }
    }
}