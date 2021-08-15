using System;
using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.ClientId;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.ClientId;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public class ClientIdService : IClientIdService
    {
        private readonly IStoreClientIdCommand _storeClientIdCommand;
        private readonly IRemoveClientIdCommand _removeClientIdCommand;
        private readonly IGetClientIdsQuery _getClientIdsQuery;

        public ClientIdService(
            IStoreClientIdCommand storeClientIdCommand,
            IRemoveClientIdCommand removeClientIdCommand,
            IGetClientIdsQuery getClientIdsQuery)
        {
            _storeClientIdCommand = storeClientIdCommand;
            _removeClientIdCommand = removeClientIdCommand;
            _getClientIdsQuery = getClientIdsQuery;
        }

        public void SaveClientId(string hubType, string clientId, string username)
        {
            _storeClientIdCommand.Store(hubType, clientId, username);
        }

        public void RemoveClientId(string hubType, string clientId)
        {
            _removeClientIdCommand.Remove(hubType, clientId);
        }

        public List<string> GetClientIds(string username, string hubType)
        {
            return _getClientIdsQuery.Get(username, hubType);
        }
    }
}