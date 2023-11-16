using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Queries.Playlist;

public class GetUsersRequestAtPlaylistIndexQuery : IGetUsersRequestAtPlaylistIndexQuery
{
    private readonly IGetCurrentRequestsRepository _getCurrentRequestsRepository;

    public GetUsersRequestAtPlaylistIndexQuery(
        IGetCurrentRequestsRepository getCurrentRequestsRepository
    )
    {
        _getCurrentRequestsRepository = getCurrentRequestsRepository;
    }

    public BasicSongRequest Get(string username, int index, bool isCurrentRequestVip)
    {
        var currentRequests = _getCurrentRequestsRepository.GetCurrentRequests();

        var requestAtIndex = currentRequests.VipRequests.Where((sr, position) =>
            sr.Username == username && isCurrentRequestVip ? index == position : index - 1 == position);

        return requestAtIndex.SingleOrDefault();
    }
}