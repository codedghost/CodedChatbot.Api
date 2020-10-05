namespace CoreCodedChatbot.ApiApplication.Interfaces.Services
{
    public interface IClientTriggerService
    {
        void TriggerSongCheck(string username);
        void SendSongToChat(string username, string title, string artist, string url);
    }
}