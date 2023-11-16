using CoreCodedChatbot.ApiApplication.Models.Intermediates;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Extensions;

public static class BasicSongRequestExtensions
{
    public static PlaylistItem CreatePlaylistItem(this BasicSongRequest request)
    {
        var playlistItem = new PlaylistItem
        {
            songRequestId = request.SongRequestId,
            songRequestText = request.SongRequestText,
            songRequester = request.Username,
            isInChat = request.IsUserInChat,
            isVip = request.IsVip,
            isSuperVip = request.IsSuperVip,
            isEvenIndex = request.IsEvenIndex,
            isInDrive = request.IsInDrive
        };

        return playlistItem;
    }
}