using System;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.ChatCommand;

public class GetCommandHelpTextByKeywordRepository : BaseRepository<InfoCommand>
{
    public GetCommandHelpTextByKeywordRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public async Task<string> Get(string keyword)
    {
        var commandKeyword = Context.InfoCommandKeywords.FirstOrDefault(ik => ik.InfoCommandKeywordText == keyword);

        if (commandKeyword == null) return string.Empty;

        var command = await GetByIdOrNullAsync(commandKeyword.InfoCommandId);

        if (command == null)
        {
            throw new Exception("Command keyword does not have a valid InfoCommandId");
        }

        return command.InfoHelpText;

    }
}