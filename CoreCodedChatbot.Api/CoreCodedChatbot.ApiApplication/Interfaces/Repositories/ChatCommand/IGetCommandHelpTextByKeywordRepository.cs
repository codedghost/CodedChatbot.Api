namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChatCommand;

public interface IGetCommandHelpTextByKeywordRepository
{
    string Get(string keyword);
}