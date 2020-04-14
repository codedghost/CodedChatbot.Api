using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes;
using CoreCodedChatbot.Database;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Bytes
{
    public class GetUserByteCountRepository : IGetUserByteCountRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetUserByteCountRepository(
            IChatbotContextFactory chatbotContextFactory
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public float Get(string username, int byteConversion)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var user = context.GetOrCreateUser(username);

                return user.TokenBytes / (float) byteConversion;
            }
        }
    }
}