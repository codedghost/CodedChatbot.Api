using CoreCodedChatbot.ApiApplication.Interfaces.Commands.Settings;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;

namespace CoreCodedChatbot.ApiApplication.Services;

public class SettingsService : ISettingsService
{
    private readonly IUpdateSettingsCommand _updateSettingsCommand;

    public SettingsService(IUpdateSettingsCommand updateSettingsCommand)
    {
        _updateSettingsCommand = updateSettingsCommand;
    }

    public void Update(string key, string value)
    {
        _updateSettingsCommand.Update(key, value);
    }
}