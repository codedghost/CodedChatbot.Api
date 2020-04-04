using System;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist
{
    public class GetIsUserInChatRepository : IGetIsUserInChatRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetIsUserInChatRepository(
            IChatbotContextFactory chatbotContextFactory
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public bool IsUserInChat(string username)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var user = context.Users.SingleOrDefault(u => u.Username == username);

                var tmiReportedInChat = user?.TimeLastInChat.AddMinutes(2) >= DateTime.UtcNow;

                return tmiReportedInChat;
            }
        }
    }
}