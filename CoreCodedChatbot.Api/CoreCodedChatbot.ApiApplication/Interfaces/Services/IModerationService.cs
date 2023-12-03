using System.Threading.Tasks;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services;

public interface IModerationService
{
    Task TransferUserAccount(string moderationUsername, string oldUsername, string newUsername);
}