using System.Threading.Tasks;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services;

public interface IChatService
{
    void Initialise();
    Task<bool> SendMessage(string message);
}