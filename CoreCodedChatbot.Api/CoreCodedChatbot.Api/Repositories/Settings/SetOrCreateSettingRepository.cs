using CoreCodedChatbot.Api.Interfaces.Repositories.Settings;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.Api.Repositories.Settings
{
    public class SetOrCreateSettingRepository : ISetOrCreateSettingRepository
    {
        private readonly IChatbotContextFactory _chatbotContextFactory;

        public SetOrCreateSettingRepository(
            IChatbotContextFactory chatbotContextFactory
            )
        {
            _chatbotContextFactory = chatbotContextFactory;
        }

        public void Set(string settingKey, string value)
        {
            using (var context = _chatbotContextFactory.Create())
            {
                var setting = context.Settings.Find(settingKey);

                if (setting == null)
                {
                    context.Settings.Add(new Setting
                    {
                        SettingName = settingKey,
                        SettingValue = value
                    });
                    return;
                }

                setting.SettingValue = value;
                context.SaveChanges();
            }
        }
    }
}