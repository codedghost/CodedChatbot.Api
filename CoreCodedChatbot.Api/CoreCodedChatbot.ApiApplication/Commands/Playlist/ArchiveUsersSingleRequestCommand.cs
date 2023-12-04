using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Vip;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Models.Enums;
using CoreCodedChatbot.Config;

namespace CoreCodedChatbot.ApiApplication.Commands.Playlist;

public class ArchiveUsersSingleRequestCommand : IArchiveUsersSingleRequestCommand
{
    private readonly IGetUsersRequestsRepository _getUsersRequestsRepository;
    private readonly IArchiveRequestCommand _archiveRequestCommand;
    private readonly IConfigService _configService;

    public ArchiveUsersSingleRequestCommand(
        IGetUsersRequestsRepository getUsersRequestsRepository,
        IArchiveRequestCommand archiveRequestCommand,
        IConfigService configService
    )
    {
        _getUsersRequestsRepository = getUsersRequestsRepository;
        _archiveRequestCommand = archiveRequestCommand;
        _configService = configService;
    }

    public async Task<bool> ArchiveAndRefundVips(string username, SongRequestType songRequestType, int currentSongRequestId)
    {
        var usersRequests = _getUsersRequestsRepository.GetUsersRequests(username);

        int songRequestId = 0;
        switch (songRequestType)
        {
            case SongRequestType.Regular:
                songRequestId = usersRequests.SingleOrDefault(r => !r.IsVip)?.SongRequestId ?? 0;
                break;
            case SongRequestType.Vip:
                songRequestId = usersRequests.SingleOrDefault(r => r.IsVip && !r.IsSuperVip)?.SongRequestId ?? 0;
                break;
            case SongRequestType.SuperVip:
                songRequestId = usersRequests.SingleOrDefault(r => r.IsSuperVip)?.SongRequestId ?? 0;
                break;
            default:
                return false;
        }

        if (songRequestId == 0 || songRequestId == currentSongRequestId) return false;

        await _archiveRequestCommand.ArchiveRequest(songRequestId, true).ConfigureAwait(false);

        return true;
    }
}