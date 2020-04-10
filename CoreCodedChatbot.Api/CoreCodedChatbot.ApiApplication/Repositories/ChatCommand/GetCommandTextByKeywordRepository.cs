using System;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChatCommand;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.ChatCommand
{
    public class GetCommandTextByKeywordRepository : IGetCommandTextByKeywordRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public GetCommandTextByKeywordRepository(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public string Get(string keyword)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var commandKeyword = context.InfoCommandKeywords.FirstOrDefault(ik => ik.InfoCommandKeywordText == keyword);

                if (commandKeyword != null)
                {
                    var command = context.InfoCommands.Single(ic => ic.InfoCommandId == commandKeyword.InfoCommandId);

                    if (command == null) throw new Exception("Command keyword does not have a valid InfoCommandId");

                    return command.InfoText;
                }
            }

            return string.Empty;
        }
    }
}