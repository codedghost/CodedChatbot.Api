using CoreCodedChatbot.Database.Context.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.ChatCommand;

public class ChatCommandsRepository : BaseRepository<InfoCommand>
{
    public ChatCommandsRepository(IChatbotContextFactory chatbotContextFactory) : base(chatbotContextFactory)
    {
    }

    public async Task Add(List<string> keywords, string informationText, string helpText, string username)
    {
        if (Context.InfoCommandKeywords.Any(ik => keywords.Contains(ik.InfoCommandKeywordText)))
        {
            throw new Exception("A command with one or more of the provided aliases already exists");
        }

        var infoCommand = new InfoCommand
        {
            InfoText = "{0}" + informationText,
            InfoHelpText = "Hey @{0}! " + helpText,
            Username = username
        };

        await CreateAsync(infoCommand);

        var infoCommandKeywords = keywords.Select(k => new InfoCommandKeyword
        {
            InfoCommandId = infoCommand.InfoCommandId,
            InfoCommandKeywordText = k
        });

        Context.InfoCommandKeywords.AddRange(infoCommandKeywords);

        await Context.SaveChangesAsync();
    }

    public async Task<string> GetCommandText(string keyword)
    {
        var commandKeyword = Context.InfoCommandKeywords.FirstOrDefault(ik => ik.InfoCommandKeywordText == keyword);

        if (commandKeyword == null) return string.Empty;

        var command = await GetByIdOrNullAsync(commandKeyword.InfoCommandId);

        if (command == null) throw new Exception("Command keyword does not have a valid InfoCommandId");

        return command.InfoText;
    }

    public async Task<string> GetHelp(string keyword)
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