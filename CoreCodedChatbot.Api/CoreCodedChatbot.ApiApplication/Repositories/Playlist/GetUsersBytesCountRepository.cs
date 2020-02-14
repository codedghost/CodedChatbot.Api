using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Playlist
{
    public class GetUsersBytesCountRepository : IGetUsersBytesCountRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;
        private readonly IConfigService _configService;

        public GetUsersBytesCountRepository(
            IChatbotContextFactory chatbotContextFactory,
            IConfigService configService
        )
        {
            _chatbotContextFactory = chatbotContextFactory;
            _configService = configService;
        }

        public string GetBytes(string username)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var user = context.Users.Find(username);

                var bytesToVip = _configService.Get<float>("BytesToVip");

                var totalBytes = user.TokenBytes / bytesToVip;

                return totalBytes.ToString("n3");
            }
        }
    }
}