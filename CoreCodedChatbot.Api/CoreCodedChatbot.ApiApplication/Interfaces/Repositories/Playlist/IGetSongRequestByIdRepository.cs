using CoreCodedChatbot.ApiApplication.Models.Intermediates;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Repositories.Playlist;

public interface IGetSongRequestByIdRepository
{
    SongRequestIntermediate GetRequest(int id);
}