using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.ClientId;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ClientId;

namespace CoreCodedChatbot.ApiApplication.Commands.ClientId
{
    public class RemoveClientIdCommand : IRemoveClientIdCommand
    {
        private readonly IRemoveClientIdRepository _removeClientIdRepository;

        public RemoveClientIdCommand(IRemoveClientIdRepository removeClientIdRepository)
        {
            _removeClientIdRepository = removeClientIdRepository;
        }

        public void Remove(string hubType, string clientId)
        {
            hubType.AssertNonNull();
            clientId.AssertNonNull();

            _removeClientIdRepository.Remove(hubType, clientId);
        }
    }
}