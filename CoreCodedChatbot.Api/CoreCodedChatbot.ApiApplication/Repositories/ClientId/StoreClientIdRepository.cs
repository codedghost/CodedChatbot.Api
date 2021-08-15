using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ClientId;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.DbExtensions;
using Microsoft.EntityFrameworkCore.Internal;

namespace CoreCodedChatbot.ApiApplication.Repositories.ClientId
{
    public class StoreClientIdRepository : IStoreClientIdRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public StoreClientIdRepository(IChatbotContextFactory chatbotContextFactory)
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void Store(string hubType, string username, string clientId)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var user = context.GetOrCreateUser(username);

                var clientIds = user.GetClientIdsDictionary();
                
                if (!clientIds.ContainsKey(hubType))
                {
                    clientIds.Add(hubType, new List<string>());
                }

                clientIds[hubType].Add(clientId);

                user.UpdateClientIdsDictionary(clientIds);

                context.SaveChanges();
            }
        }
    }
}