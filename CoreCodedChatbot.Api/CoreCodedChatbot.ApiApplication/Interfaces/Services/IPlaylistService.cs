using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiContract.Enums.Playlist;
using CoreCodedChatbot.ApiContract.RequestModels.Playlist;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services
{
    public interface IPlaylistService
    {
        PlaylistItem GetRequestById(int songId);
        Task<(AddRequestResult, int)> AddRequest(string username, string commandText, bool vipRequest = false);
        Task<AddRequestResult> AddSuperVipRequest(string username, string commandText);
        Task<AddRequestResult> AddWebRequest(AddWebSongRequest requestSongViewModel, string username);
        PlaylistState GetPlaylistState();
        Task<PromoteSongResponse> PromoteRequest(string username, int songId, bool useSuperVip);
        void UpdateFullPlaylist(bool updateCurrent = false);
        void ArchiveCurrentRequest(int songId = 0);
        string GetUserRequests(string username);
        GetAllSongsResponse GetAllSongs();
        Task ClearRockRequests();
        Task<bool> RemoveRockRequests(string username, string commandText, bool isMod);

        bool EditRequest(string username, string commandText, bool isMod, out string songRequestText,
            out bool syntaxError);

        EditRequestResult EditWebRequest(EditWebRequestRequestModel editWebRequestRequestModel);

        bool AddSongToDrive(int songId);

        Task<bool> OpenPlaylist();
        Task<bool> ClosePlaylist();
        Task<bool> ArchiveRequestById(int songId);
        Task<bool> VeryClosePlaylist();
        int GetMaxUserRequests();
        bool EditSuperVipRequest(string username, string songText);
        Task<bool> RemoveSuperRequest(string username);
        bool IsSuperVipRequestInQueue();
        PlaylistItem GetCurrentSongRequest();
        List<PlaylistItem> GetTopTenPlaylistItems();
    }
}