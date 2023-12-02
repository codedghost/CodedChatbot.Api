using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.Abstractions;
using CoreCodedChatbot.Database.Context.Interfaces;
using CoreCodedChatbot.Database.Context.Models;

namespace CoreCodedChatbot.ApiApplication.Repositories.Settings;

public class SetOrCreateSettingRepository : BaseRepository<Setting>
{
    public SetOrCreateSettingRepository(
        IChatbotContextFactory chatbotContextFactory
    ) : base(chatbotContextFactory)
    {
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