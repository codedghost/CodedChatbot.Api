using System.Collections.Generic;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.ChatCommand;

public interface IAddChatCommandCommand
{
    void Add(List<string> keywords, string informationText, string helpText, string username);
}