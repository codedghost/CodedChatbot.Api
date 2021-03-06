﻿using System.Collections.Generic;
using CoreCodedChatbot.ApiContract.Enums.Playlist;
using CoreCodedChatbot.ApiContract.RequestModels.Playlist;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist.ChildModels;

namespace CoreCodedChatbot.ApiApplication.Interfaces.Services
{
    public interface IPlaylistService
    {
        PlaylistItem GetRequestById(int songId);
        (AddRequestResult, int) AddRequest(string username, string commandText, bool vipRequest = false);
        AddRequestResult AddSuperVipRequest(string username, string commandText);
        AddRequestResult AddWebRequest(AddWebSongRequest requestSongViewModel, string username);
        PlaylistState GetPlaylistState();
        PromoteSongResponse PromoteRequest(string username, int songId);
        void UpdateFullPlaylist(bool updateCurrent = false);
        void ArchiveCurrentRequest(int songId = 0);
        string GetUserRequests(string username);
        GetAllSongsResponse GetAllSongs();
        void ClearRockRequests();
        bool RemoveRockRequests(string username, string commandText, bool isMod);

        bool EditRequest(string username, string commandText, bool isMod, out string songRequestText,
            out bool syntaxError);

        EditRequestResult EditWebRequest(EditWebRequestRequestModel editWebRequestRequestModel);

        bool AddSongToDrive(int songId);

        bool OpenPlaylist();
        bool ClosePlaylist();
        bool ArchiveRequestById(int songId);
        bool VeryClosePlaylist();
        int GetMaxUserRequests();
        bool EditSuperVipRequest(string username, string songText);
        bool RemoveSuperRequest(string username);
        bool IsSuperVipRequestInQueue();
        PlaylistItem GetCurrentSongRequest();
        List<PlaylistItem> GetTopTenPlaylistItems();
    }
}