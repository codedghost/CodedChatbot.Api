namespace CoreCodedChatbot.ApiApplication.Interfaces.Services
{
    public interface IChatCommandService
    {
        string GetCommandText(string keyword);
        string GetCommandHelpText(string keyword);
    }
}