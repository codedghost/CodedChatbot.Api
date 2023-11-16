using System.Collections.Generic;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Queries.Playlist;

public class GetCurrentRequestsQuery : IGetCurrentRequestsQuery
{
    private readonly IGetCurrentRequestsRepository _getCurrentRequestsRepository;

    public GetCurrentRequestsQuery(
        IGetCurrentRequestsRepository getCurrentRequestsRepository
    )
    {
        _getCurrentRequestsRepository = getCurrentRequestsRepository;
    }

    public (List<PlaylistItem> RegularRequests, List<PlaylistItem> VipRequests) GetCurrentRequests()
    {
        var currentRequests = _getCurrentRequestsRepository.GetCurrentRequests();

        var regularRequests = currentRequests.RegularRequests.Select(r => r.CreatePlaylistItem()).ToList();
        var vipRequests = currentRequests.VipRequests.Select(r => r.CreatePlaylistItem()).ToList();

        return (regularRequests, vipRequests);
    }
}