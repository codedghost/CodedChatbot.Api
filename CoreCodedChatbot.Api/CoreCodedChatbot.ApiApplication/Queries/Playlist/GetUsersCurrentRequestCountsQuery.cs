using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Vip;
using CoreCodedChatbot.ApiApplication.Models.Enums;

namespace CoreCodedChatbot.ApiApplication.Queries.Playlist
{
    public class GetUsersCurrentRequestCountsQuery : IGetUsersCurrentRequestCountsQuery
    {
        private readonly IGetUsersCurrentRegularRequestCountRepository _getUsersCurrentRegularRequestCountRepository;
        private readonly IGetUsersCurrentVipRequestCountRepository _getUsersCurrentVipRequestCountRepository;
        private readonly IGetUsersCurrentSuperVipRequestCountRepository _getUsersCurrentSuperVipRequestCountRepository;
        private readonly IGetUsersCurrentRequestCountRepository _getUsersCurrentRequestCountRepository;

        public GetUsersCurrentRequestCountsQuery(
            IGetUsersCurrentRegularRequestCountRepository getUsersCurrentRegularRequestCountRepository,
            IGetUsersCurrentVipRequestCountRepository  getUsersCurrentVipRequestCountRepository,
            IGetUsersCurrentSuperVipRequestCountRepository getUsersCurrentSuperVipRequestCountRepository,
            IGetUsersCurrentRequestCountRepository getUsersCurrentRequestCountRepository
            )
        {
            _getUsersCurrentRegularRequestCountRepository = getUsersCurrentRegularRequestCountRepository;
            _getUsersCurrentVipRequestCountRepository = getUsersCurrentVipRequestCountRepository;
            _getUsersCurrentSuperVipRequestCountRepository = getUsersCurrentSuperVipRequestCountRepository;
            _getUsersCurrentRequestCountRepository = getUsersCurrentRequestCountRepository;
        }

        public int GetUsersCurrentRequestCounts(string username, SongRequestType songRequestType)
        {
            switch (songRequestType)
            {
                case SongRequestType.Regular:
                    return _getUsersCurrentRegularRequestCountRepository.GetUsersCurrentRegularRequestCount(username);
                case SongRequestType.Vip:
                    return _getUsersCurrentVipRequestCountRepository.GetUsersCurrentVipRequestCount(username);
                case SongRequestType.SuperVip:
                    return _getUsersCurrentSuperVipRequestCountRepository.GetUsersCurrentSuperVipRequestCount(username);
                case SongRequestType.Any:
                    return _getUsersCurrentRequestCountRepository.GetUsersCurrentRequestCount(username);
                default:
                    return _getUsersCurrentRequestCountRepository.GetUsersCurrentRequestCount(username);
            }
        }
    }
}