using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist
{
    public interface IProcessSuperVipSongRequestCommand
    {
        Task<AddSongResult> Process(string username, string requestText, int searchSongId);
    }
}