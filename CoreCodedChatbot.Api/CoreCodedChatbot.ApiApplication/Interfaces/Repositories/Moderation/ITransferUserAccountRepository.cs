namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Moderation;

public interface ITransferUserAccountRepository
{
    void Transfer(string moderatorUsername, string oldUsername, string newUsername);
}