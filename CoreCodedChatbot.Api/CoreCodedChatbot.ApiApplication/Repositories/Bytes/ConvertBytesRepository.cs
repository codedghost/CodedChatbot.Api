using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Bytes;
using CoreCodedChatbot.Database;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.DbExtensions;

namespace CoreCodedChatbot.ApiApplication.Repositories.Bytes
{
    public class ConvertBytesRepository : IConvertBytesRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public ConvertBytesRepository(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public int Convert(string username, int tokensToConvert, int byteConversion)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var user = context.GetOrCreateUser(username);

                if (tokensToConvert < 0 || (user.TokenBytes < byteConversion * tokensToConvert)) return 0;

                var bytesToRemove = byteConversion * tokensToConvert;

                user.TokenBytes -= bytesToRemove;
                user.TokenVipRequests += tokensToConvert;

                context.SaveChanges();

                return tokensToConvert;
            }
        }
    }
}