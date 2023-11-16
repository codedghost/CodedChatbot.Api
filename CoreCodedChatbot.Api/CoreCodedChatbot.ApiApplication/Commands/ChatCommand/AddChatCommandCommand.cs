using System.Collections.Generic;
using CoreCodedChatbot.ApiApplication.Interfaces.Commands.ChatCommand;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChatCommand;

namespace CoreCodedChatbot.ApiApplication.Commands.ChatCommand;

public class AddChatCommandCommand : IAddChatCommandCommand
{
    private readonly IAddChatCommandRepository _addChatCommandRepository;

    public AddChatCommandCommand(IAddChatCommandRepository addChatCommandRepository)
    {
        _addChatCommandRepository = addChatCommandRepository;
    }

    public void Add(List<string> keywords, string informationText, string helpText, string username)
    {
        _addChatCommandRepository.Add(keywords, informationText, helpText, username);
    }
}