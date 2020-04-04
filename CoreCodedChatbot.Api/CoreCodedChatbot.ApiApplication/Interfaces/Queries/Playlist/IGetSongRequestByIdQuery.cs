using CoreCodedChatbot.ApiContract.ResponseModels.Playlist.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Queries.Playlist
{
    public interface IGetSongRequestByIdQuery
    {
        PlaylistItem GetSongRequestById(int id);
    }
}