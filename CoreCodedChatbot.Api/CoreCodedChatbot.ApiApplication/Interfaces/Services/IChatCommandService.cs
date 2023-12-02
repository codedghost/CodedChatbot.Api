using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services;

public interface IChatCommandService
{
    Task<string> GetCommandText(string keyword);
    Task<string> GetCommandHelpText(string keyword);
    Task AddCommand(List<string> keywords, string informationText, string helpText, string username);
}