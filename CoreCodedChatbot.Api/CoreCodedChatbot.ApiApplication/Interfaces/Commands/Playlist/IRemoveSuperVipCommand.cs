using System.Threading.Tasks;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist
{
    public interface IRemoveSuperVipCommand
    {
        Task Remove(string username);
    }
}