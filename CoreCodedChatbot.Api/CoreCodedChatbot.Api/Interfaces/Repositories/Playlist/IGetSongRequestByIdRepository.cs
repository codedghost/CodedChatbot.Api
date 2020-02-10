using CoreCodedChatbot.Api.Models.Intermediates;

namespace CoreCodedChatbot.Api.Interfaces.Repositories.Playlist
{
    public interface IGetSongRequestByIdRepository
    {
        SongRequestIntermediate GetRequest(int id);
    }
}