using CoreCodedChatbot.Api.Interfaces.Queries.Playlist;
using CoreCodedChatbot.Api.Models.Enums;
using CoreCodedChatbot.Config;

namespace CoreCodedChatbot.Api.Queries.Playlist
{
    public class CheckUserHasMaxRegularsInQueueQuery : ICheckUserHasMaxRegularsInQueueQuery
    {
        private readonly IGetUsersCurrentRequestCountsQuery _getUsersCurrentRequestCountsQuery;
        private readonly IConfigService _configService;

        public CheckUserHasMaxRegularsInQueueQuery(
            IGetUsersCurrentRequestCountsQuery getUsersCurrentRequestCountsQuery,
            IConfigService configService
            )
        {
            _getUsersCurrentRequestCountsQuery = getUsersCurrentRequestCountsQuery;
            _configService = configService;
        }

        public bool UserHasMaxRegularsInQueue(string username)
        {
            var maxRegulars = _configService.Get<int>("MaxRegularSongsPerUser");

            var usersRegulars =
                _getUsersCurrentRequestCountsQuery.GetUsersCurrentRequestCounts(username, SongRequestType.Regular);

            return usersRegulars >= maxRegulars;
        }
    }
}