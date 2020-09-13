namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Settings
{
    public interface IUpdateSettingsCommand
    {
        void Update(string key, string value);
    }
}