using System;
using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Bytes
{
    public class GiveViewershipBytesRepository : IGiveViewershipBytesRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GiveViewershipBytesRepository(IChatbotContextFactory chatbotContextFactory)
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void Give(List<string> usernames)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                foreach (var username in usernames)
                {
                    var user = context.GetOrCreateUser(username, true);

                    //user.TokenBytes++;
                    user.TimeLastInChat = DateTime.UtcNow;
                }

                context.SaveChanges();
            }
        }
    }
}