using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.ChatCommand;

public class AddChatCommandRepository : BaseRepository<InfoCommand>
{
    public AddChatCommandRepository(IChatbotContextFactory chatbotContextFactory)
        : base(chatbotContextFactory)
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
}