namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.ChatCommand;

public interface IGetCommandHelpTextByKeywordQuery
{
    string Get(string keyword);
}