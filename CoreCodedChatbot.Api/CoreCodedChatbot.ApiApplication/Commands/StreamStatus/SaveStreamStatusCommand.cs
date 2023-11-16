using CoreCodedChatbot.ApiApplication.Interfaces.Commands.StreamStatus;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.StreamStatus;
using CoreCodedChatbot.ApiContract.RequestModels.StreamStatus;

namespace CoreCodedChatbot.ApiApplication.Commands.StreamStatus;

public class SaveStreamStatusCommand : ISaveStreamStatusCommand
{
    private readonly ISaveStreamStatusRepository _saveStreamStatusRepository;

    public SaveStreamStatusCommand(
        ISaveStreamStatusRepository saveStreamStatusRepository
    )
    {
        _saveStreamStatusRepository = saveStreamStatusRepository;
    }

    public bool Save(PutStreamStatusRequest request)
    {
        var result = _saveStreamStatusRepository.Save(request);

        return result;
    }
}