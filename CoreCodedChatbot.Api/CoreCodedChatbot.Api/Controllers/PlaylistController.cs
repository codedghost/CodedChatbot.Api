﻿using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.RequestModels.Playlist;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api.Controllers
{
    [Route("Playlist/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlaylistController : Controller
    {
        private readonly IPlaylistService _playlistService;
        private readonly ILogger<PlaylistController> _logger;

        public PlaylistController(
            IPlaylistService playlistService,
            ILogger<PlaylistController> logger
            )
        {
            _playlistService = playlistService;
            _logger = logger;
        }

        public IActionResult TestEndpoint()
        {
            return new JsonResult(new {Message = "Authorized!"});
        }

        [HttpPost]
        public IActionResult EditRequest([FromBody] EditSongRequest editSongRequest)
        {
            var success = _playlistService.EditRequest(editSongRequest.Username, editSongRequest.CommandText, editSongRequest.IsMod, 
                out string songRequestText, out bool syntaxError);

            if (success)
            {
                var editResult = new EditRequestResponse
                {
                    SongRequestText = songRequestText,
                    SyntaxError = syntaxError
                };
                return new JsonResult(editResult);
            }
            else
                return BadRequest();
        }

        [HttpPost]
        public IActionResult GetUserRequests([FromBody] string username)
        {
            var requests = _playlistService.GetUserRequests(username);

            var requestsResult = new GetUserRequestsResponse
            {
                UserRequests = requests
            };

            return new JsonResult(requestsResult);
        }

        public IActionResult OpenPlaylist()
        {
            if (_playlistService.OpenPlaylist())
                return Ok();

            return BadRequest();
        }

        public IActionResult VeryClosePlaylist()
        {
            if (_playlistService.VeryClosePlaylist())
                return Ok();

            return BadRequest();
        }

        public IActionResult ClosePlaylist()
        {
            if (_playlistService.ClosePlaylist())
                return Ok();

            return BadRequest();
        }

        public IActionResult IsPlaylistOpen()
        {
            return Json(_playlistService.GetPlaylistState());
        }

        public IActionResult ArchiveCurrentRequest()
        {
            try
            {
                _playlistService.ArchiveCurrentRequest();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult RemoveRockRequests([FromBody] RemoveSongRequest removeSongRequest)
        {
            if (_playlistService.RemoveRockRequests(removeSongRequest.Username, removeSongRequest.CommandText, removeSongRequest.IsMod))
                return Ok();

            return BadRequest();
        }

        [HttpPost]
        public IActionResult RemoveSuperVip([FromBody] RemoveSuperVipRequest requestModel)
        {
            if (_playlistService.RemoveSuperRequest(requestModel.Username)) return Ok();

            return BadRequest();
        }

        [HttpPost]
        public IActionResult AddRequest([FromBody] AddSongRequest requestModel)
        {
            var addRequestResult = _playlistService.AddRequest(requestModel.Username, requestModel.CommandText, requestModel.IsVipRequest);
            return new JsonResult(new AddRequestResponse
            {
                Result = addRequestResult.Item1,
                PlaylistPosition = addRequestResult.Item2
            });
        }

        [HttpPost]
        public IActionResult AddWebRequest([FromBody] AddWebSongRequest addWebSongRequest)
        {
            try
            {
                var requestSongViewModel = new AddWebSongRequest
                {
                    Title = addWebSongRequest.Title,
                    Artist = addWebSongRequest.Artist,
                    SelectedInstrument = addWebSongRequest.SelectedInstrument,
                    IsVip = addWebSongRequest.IsVip,
                    IsSuperVip = addWebSongRequest.IsSuperVip
                };

                var result = _playlistService.AddWebRequest(requestSongViewModel, addWebSongRequest.Username);

                var responseModel = new AddRequestResponse
                {
                    Result = result,
                    PlaylistPosition = 0
                };

                return new JsonResult(responseModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in AddWebRequest", addWebSongRequest);
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult AddSuperRequest([FromBody] AddSuperVipRequest requestModel)
        {
            try
            {
                var addSuperVipResult =
                    _playlistService.AddSuperVipRequest(requestModel.Username, requestModel.CommandText);

                return new JsonResult(new AddRequestResponse
                {
                    Result = addSuperVipResult
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in AddSuperRequest", requestModel);
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult EditSuperVipRequest([FromBody] EditSuperVipRequest requestModel)
        {
            try
            {
                var editSuperVipResult =
                    _playlistService.EditSuperVipRequest(requestModel.Username, requestModel.CommandText);

                if (editSuperVipResult)
                    return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in EditSuperVipRequest", requestModel);
            }

            return BadRequest();
        }

        [HttpPost]
        public IActionResult PromoteRequest([FromBody] PromoteSongRequest promoteSongRequest)
        {
            try
            {
                var result =
                    _playlistService.PromoteRequest(promoteSongRequest.Username, promoteSongRequest.SongRequestId);

                return new JsonResult(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in PromoteWebRequest", promoteSongRequest);
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult ClearRequests()
        {
            try
            {
                _playlistService.ClearRockRequests();
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in ClearRequests");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetAllSongs()
        {
            try
            {
                return new JsonResult(_playlistService.GetAllSongs());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in GetAllSongs");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetRequestById(int songId)
        {
            try
            {
                var request = _playlistService.GetRequestById(songId);

                var responseModel = new GetRequestByIdResponse
                {
                    Request = request
                };

                return new JsonResult(responseModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in GetRequestById", songId);
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult ArchiveRequestById(int songId)
        {
            try
            {
                var result = _playlistService.ArchiveRequestById(songId);

                return new JsonResult(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in ArchiveRequestById", songId);
                return BadRequest();
            }
        }

        [HttpDelete]
        public IActionResult ArchiveCurrentRequest(int songId)
        {
            try
            {
                _playlistService.ArchiveCurrentRequest(songId);

                return new JsonResult(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in ArchiveCurrentRequest", songId);
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetMaxUserRequests()
        {
            try
            {
                var result = _playlistService.GetMaxUserRequests();

                var responseModel = new MaxUserRequestsResponse
                {
                    MaxRequests = result
                };

                return new JsonResult(responseModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in GetMaxUserRequests");
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult EditWebRequest([FromBody] EditWebRequestRequestModel editWebRequestRequestModel)
        {
            try
            {
                var result = _playlistService.EditWebRequest(editWebRequestRequestModel);

                // Need a new EditWebRequestResponse model to hold the edit result enum
                var responseModel = new EditWebRequestResponse
                {
                    EditResult = result
                };

                return new JsonResult(responseModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in EditWebRequest", editWebRequestRequestModel);
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult AddRequestToDrive([FromBody] AddSongToDriveRequest addSongToDriveRequest)
        {
            try
            {
                var result = _playlistService.AddSongToDrive(addSongToDriveRequest.SongRequestId);

                if (result) return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in AddRequestToDrive", addSongToDriveRequest);
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult GetCurrentSongRequest()
        {
            try
            {
                var songRequest = _playlistService.GetCurrentSongRequest();

                if (songRequest == null) return BadRequest();

                var responseModel = songRequest.FormattedRequest == null
                    ? new GetCurrentSongRequestResponse
                    {
                        SongArtist = string.Empty,
                        SongName = songRequest.songRequestText,
                        RequesterUsername = songRequest.songRequester,
                        InstrumentName = string.Empty
                    }
                    : new GetCurrentSongRequestResponse
                    {
                        SongArtist = songRequest.FormattedRequest.SongArtist,
                        SongName = songRequest.FormattedRequest.SongName,
                        RequesterUsername = songRequest.songRequester,
                        InstrumentName = songRequest.FormattedRequest.InstrumentName
                    };

                return new JsonResult(responseModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in GetCurrentSongRequest");
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult GetTopTen()
        {
            try
            {
                var topRequests = _playlistService.GetTopTenPlaylistItems();

                return new JsonResult(topRequests);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in GetTopTen");
            }

            return BadRequest();
        }
    }
}
