using CoreCodedChatbot.ApiApplication.Interfaces.Queries.ChatCommand;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChatCommand;

namespace CoreCodedChatbot.ApiApplication.Queries.ChatCommand;

public class GetCommandTextByKeywordQuery : IGetCommandTextByKeywordQuery
{
    private readonly IGetCommandTextByKeywordRepository _getCommandTextByKeywordRepository;

    public GetCommandTextByKeywordQuery(IGetCommandTextByKeywordRepository getCommandTextByKeywordRepository)
    {
        _getCommandTextByKeywordRepository = getCommandTextByKeywordRepository;
    }

    public string Get(string keyword)
    {
        var infoText = _getCommandTextByKeywordRepository.Get(keyword);

        return infoText;
    }
}