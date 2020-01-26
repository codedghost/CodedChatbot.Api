namespace CoreCodedChatbot.Api.Interfaces.Repositories.Settings
{
    public interface IGetSettingRepository
    {
        T Get<T>(string settingKey);
    }
}