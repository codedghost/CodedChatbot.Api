using CoreCodedChatbot.ApiApplication.Interfaces.Queries.ChatCommand;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChatCommand;

namespace CoreCodedChatbot.ApiApplication.Queries.ChatCommand
{
    public class GetCommandHelpTextByKeywordQuery : IGetCommandHelpTextByKeywordQuery
    {
        private readonly IGetCommandHelpTextByKeywordRepository _getCommandHelpTextByKeywordRepository;

        public GetCommandHelpTextByKeywordQuery(
            IGetCommandHelpTextByKeywordRepository getCommandHelpTextByKeywordRepository
            )
        {
            _getCommandHelpTextByKeywordRepository = getCommandHelpTextByKeywordRepository;
        }

        public string Get(string keyword)
        {
            var helpText = _getCommandHelpTextByKeywordRepository.Get(keyword);

            return helpText;
        }
    }
}