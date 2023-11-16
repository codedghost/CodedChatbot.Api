namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.ChatCommand;

public interface IGetCommandTextByKeywordQuery
{
    string Get(string keyword);
}