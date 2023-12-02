using System;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Settings;

public class GetSettingRepository : BaseRepository<Setting>
{
    public GetSettingRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
    }

    public T Get<T>(string settingKey)
    {
        var setting = Context.Settings.SingleOrDefault(s => s.SettingName == settingKey);

        if (setting == null)
            return default;

        var castValue = Convert.ChangeType(setting.SettingValue, typeof(T));

        return (T) castValue;
    }
}