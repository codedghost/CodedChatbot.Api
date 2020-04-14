using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.ChatCommand;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.ChatCommand;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;

namespace CoreCodedChatbot.ApiApplication.Services
{
    public class ChatCommandService : IChatCommandService
    {
        private readonly IGetCommandTextByKeywordQuery _getCommandTextByKeywordQuery;
        private readonly IGetCommandHelpTextByKeywordQuery _getCommandHelpTextByKeywordQuery;
        private readonly IAddChatCommandCommand _addChatCommandCommand;

        public ChatCommandService(
            IGetCommandTextByKeywordQuery getCommandTextByKeywordQuery,
            IGetCommandHelpTextByKeywordQuery getCommandHelpTextByKeywordQuery,
            IAddChatCommandCommand addChatCommandCommand
            )
        {
            _getCommandTextByKeywordQuery = getCommandTextByKeywordQuery;
            _getCommandHelpTextByKeywordQuery = getCommandHelpTextByKeywordQuery;
            _addChatCommandCommand = addChatCommandCommand;
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

        public void AddCommand(List<string> keywords, string informationText, string helpText, string username)
        {
            _addChatCommandCommand.Add(keywords, informationText, helpText, username);
        }
    }
}