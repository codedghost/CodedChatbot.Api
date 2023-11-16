using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiContract.Enums.Playlist;
using CoreCodedChatbot.Config;

namespace CoreCodedChatbot.ApiApplication.Commands.Playlist;

public class ProcessRegularSongRequestCommand : IProcessRegularSongRequestCommand
{
    private readonly IConfigService _configService;
    private readonly ICheckUserHasMaxRegularsInQueueQuery _checkUserHasMaxRegularsInQueueQuery;
    private readonly IAddRequestRepository _addRequestRepository;

    public ProcessRegularSongRequestCommand(
        IConfigService configService,
        ICheckUserHasMaxRegularsInQueueQuery checkUserHasMaxRegularsInQueueQuery,
        IAddRequestRepository addRequestRepository
    )
    {
        _configService = configService;
        _checkUserHasMaxRegularsInQueueQuery = checkUserHasMaxRegularsInQueueQuery;
        _addRequestRepository = addRequestRepository;
    }

    public AddSongResult Process(string username, string requestText, int searchSongId)
    {
        var maxRegulars = _configService.Get<int>("MaxRegularSongsPerUser");

        if (_checkUserHasMaxRegularsInQueueQuery.UserHasMaxRegularsInQueue(username))
            return new AddSongResult
            {
                AddRequestResult = AddRequestResult.MaximumRegularRequests,
                MaximumRegularRequests = maxRegulars
            };

        return _addRequestRepository.AddRequest(requestText, username, false, false, searchSongId);
    }
}