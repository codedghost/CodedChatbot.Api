using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Models.Enums;
using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Commands.Playlist
{
    public interface IAddSongRequestCommand
    {
        Task<AddSongResult> AddSongRequest(string username, string requestText, SongRequestType songRequestType, int searchSongId);
    }
}