using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Settings;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Settings;

namespace CoreCodedChatbot.ApiApplication.Commands.Settings;

public class UpdateSettingsCommand : IUpdateSettingsCommand
{
    private readonly ISetOrCreateSettingRepository _setOrCreateSettingRepository;

    public UpdateSettingsCommand(ISetOrCreateSettingRepository setOrCreateSettingRepository)
    {
        _setOrCreateSettingRepository = setOrCreateSettingRepository;
    }

    public void Update(string key, string value)
    {
        _setOrCreateSettingRepository.Set(key, value);
    }
}