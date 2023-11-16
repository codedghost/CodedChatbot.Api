using System;
using System.Linq;
using CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Settings;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.ApiApplication.Repositories.Settings;

public class GetSettingRepository : IGetSettingRepository
{
    private readonly IChatbotContextFactory _chatbotContextFactory;

    public GetSettingRepository(
        IChatbotContextFactory chatbotContextFactory
    )
    {
        _chatbotContextFactory = chatbotContextFactory;
    }

    public T Get<T>(string settingKey)
    {
        using (var context = _chatbotContextFactory.Create())
        {
            var setting = context.Settings.SingleOrDefault(s => s.SettingName == settingKey);

            if (setting == null)
                return default;

            var castValue = Convert.ChangeType(setting.SettingValue, typeof(T));

            return (T)castValue;
        }
    }
}