namespace CoreCodedChatbot.ApiApplication.Interfaces.Services;

public interface IModerationService
{
    void TransferUserAccount(string moderationUsername, string oldUsername, string newUsername);
}