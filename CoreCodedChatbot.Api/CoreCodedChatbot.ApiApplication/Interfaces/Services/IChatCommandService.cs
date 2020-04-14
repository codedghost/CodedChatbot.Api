using System.Collections.Generic;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services
{
    public interface IChatCommandService
    {
        string GetCommandText(string keyword);
        string GetCommandHelpText(string keyword);
        void AddCommand(List<string> keywords, string informationText, string helpText, string username);
    }
}