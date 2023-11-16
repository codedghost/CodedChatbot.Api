using System.Collections.Generic;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChatCommand;

public interface IAddChatCommandRepository
{
    void Add(List<string> keywords, string informationText, string helpText, string username);
}