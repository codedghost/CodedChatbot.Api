using System.Threading.Tasks;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist
{
    public interface IArchiveRequestCommand
    {
        Task ArchiveRequest(int requestId, bool refundVip);
    }
}