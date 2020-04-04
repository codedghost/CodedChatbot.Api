using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.Config;

namespace CoreCodedChatbot.ApiApplication.Queries.Playlist
{
    public class GetMaxRegularRequestCountQuery : IGetMaxRegularRequestCountQuery
    {
        private readonly IConfigService _configService;

        public GetMaxRegularRequestCountQuery(IConfigService configService)
        {
            _configService = configService;
        }

        public int Get()
        {
            var maxRegularRequests = _configService.Get<int>("MaxRegularSongsPerUser");

            return maxRegularRequests;
        }
    }
}