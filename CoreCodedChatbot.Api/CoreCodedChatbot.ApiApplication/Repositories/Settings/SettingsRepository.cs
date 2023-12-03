using System.Linq;
using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Settings;

public class SettingsRepository : BaseRepository<Setting>
{
    public SettingsRepository(IChatbotContextFactory chatbotContextFactory) : base(chatbotContextFactory)
    {
    }

    public T Get<T>(string settingKey)
    {
        var setting = Context.Settings.SingleOrDefault(s => s.SettingName == settingKey);

        if (setting == null)
            return default;

        var castValue = Convert.ChangeType(setting.SettingValue, typeof(T));

        return (T)castValue;
    }

    public async Task Set(string settingKey, string value)
    {
        var setting = Context.Settings.SingleOrDefault(s => s.SettingName == settingKey);

        if (setting == null)
        {
            await CreateAndSaveAsync(new Setting
            {
                SettingName = settingKey,
                SettingValue = value
            });
            return;
        }

        setting.SettingValue = value;
        await Context.SaveChangesAsync();
    }
}