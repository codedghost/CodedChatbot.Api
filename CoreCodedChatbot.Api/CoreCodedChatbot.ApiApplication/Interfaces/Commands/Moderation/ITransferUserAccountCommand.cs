namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Moderation;

public interface ITransferUserAccountCommand
{
    void Transfer(string moderatorUsername, string oldUsername, string newUsername);
}