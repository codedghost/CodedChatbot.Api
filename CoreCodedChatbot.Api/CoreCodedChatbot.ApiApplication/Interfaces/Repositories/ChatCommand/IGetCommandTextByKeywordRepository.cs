namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.ChatCommand
{
    public interface IGetCommandTextByKeywordRepository
    {
        string Get(string keyword);
    }
}