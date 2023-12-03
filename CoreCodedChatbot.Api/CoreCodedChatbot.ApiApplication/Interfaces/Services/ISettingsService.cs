using System.Threading.Tasks;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services;

public interface ISettingsService
{
    Task Update(string key, string value);
}