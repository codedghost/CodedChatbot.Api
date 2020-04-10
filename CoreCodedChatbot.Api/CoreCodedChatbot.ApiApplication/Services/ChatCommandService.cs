using CoreCodedChatbot.ApiApplication.Interfaces.Queries.ChatCommand;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public class ChatCommandService : IChatCommandService
    {
        private readonly IGetCommandTextByKeywordQuery _getCommandTextByKeywordQuery;
        private readonly IGetCommandHelpTextByKeywordQuery _getCommandHelpTextByKeywordQuery;

        public ChatCommandService(
            IGetCommandTextByKeywordQuery getCommandTextByKeywordQuery,
            IGetCommandHelpTextByKeywordQuery getCommandHelpTextByKeywordQuery
            )
        {
            _getCommandTextByKeywordQuery = getCommandTextByKeywordQuery;
            _getCommandHelpTextByKeywordQuery = getCommandHelpTextByKeywordQuery;
        }

        public string GetCommandText(string keyword)
        {
            var commandText = _getCommandTextByKeywordQuery.Get(keyword);

            return commandText;
        }

        public string GetCommandHelpText(string keyword)
        {
            var commandHelpText = _getCommandHelpTextByKeywordQuery.Get(keyword);

            return commandHelpText;
        }
    }
}