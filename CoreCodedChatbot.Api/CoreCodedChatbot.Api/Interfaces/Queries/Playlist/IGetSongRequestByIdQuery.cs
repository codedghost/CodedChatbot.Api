using CoreCodedChatbot.ApiContract.ResponseModels.Playlist.ChildModels;

namespace CoreCodedChatbot.Api.Interfaces.Queries.Playlist
{
    public interface IGetSongRequestByIdQuery
    {
        PlaylistItem GetSongRequestById(int id);
    }
}