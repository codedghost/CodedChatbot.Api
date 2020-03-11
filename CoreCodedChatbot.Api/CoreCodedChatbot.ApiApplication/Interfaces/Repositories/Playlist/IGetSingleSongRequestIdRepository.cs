using CoreCodedChatbot.ApiApplication.Models.Enums;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist
{
    public interface IGetSingleSongRequestIdRepository
    {
        int Get(string username, SongRequestType songRequestType);
    }
}