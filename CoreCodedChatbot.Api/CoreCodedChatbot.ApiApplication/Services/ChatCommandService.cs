using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Repositories.ChatCommand;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Services;

public class ChatCommandService : IBaseService, IChatCommandService
{
    private readonly IChatbotContextFactory _chatbotContextFactory;

    public ChatCommandService(
        IChatbotContextFactory chatbotContextFactory
    )
    {
        _chatbotContextFactory = chatbotContextFactory;
    }

    public async Task<string> GetCommandText(string keyword)
    {
        using (var repo = new ChatCommandsRepository(_chatbotContextFactory))
        {
            return await repo.GetCommandText(keyword);
        }
    }

    public async Task<string> GetCommandHelpText(string keyword)
    {
        using (var repo = new ChatCommandsRepository(_chatbotContextFactory))
        {
            return await repo.GetHelp(keyword);
        }
    }

    public async Task AddCommand(List<string> keywords, string informationText, string helpText, string username)
    {
        using (var repo = new ChatCommandsRepository(_chatbotContextFactory))
        {
            await repo.Add(keywords, informationText, helpText, username);
        }
    }
}