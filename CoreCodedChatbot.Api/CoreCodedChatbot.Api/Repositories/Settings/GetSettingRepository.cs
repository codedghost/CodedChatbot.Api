using System;
using CoreCodedChatbot.Api.Interfaces.Repositories.Settings;
using CoreCodedChatbot.Database.Context.Interfaces;

namespace CoreCodedChatbot.Api.Repositories.Settings
{
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
                var setting = context.Settings.Find(settingKey);

                if (setting == null)
                    return default;

                var castValue = Convert.ChangeType(setting.SettingValue, typeof(T));

                return (T)castValue;
            }
        }
    }
}