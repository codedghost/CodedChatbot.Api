namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Settings;

public interface ISetOrCreateSettingRepository
{
    void Set(string settingKey, string value);
}