using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Search
{
    public interface IGetSongBySearchIdRepository
    {
        SongSearchIntermediate Get(int songId);
    }
}