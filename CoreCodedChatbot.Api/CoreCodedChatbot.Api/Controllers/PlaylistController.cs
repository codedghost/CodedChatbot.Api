using System;
using CoreCodedChatbot.ApiContract.RequestModels.Playlist;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist;
using CoreCodedChatbot.Library.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodedChatbot.Api.Controllers
{
    [Route("Playlist/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlaylistController : Controller
    {
        private readonly IPlaylistService _playlistService;

        public PlaylistController(IPlaylistService playlistService)
        {
            _playlistService = playlistService;
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
        public IActionResult AddSuperRequest([FromBody] AddSuperVipRequest requestModel)
        {
            var addSuperVipResult = _playlistService.AddSuperVipRequest(requestModel.Username, requestModel.CommandText);

            return new JsonResult(new AddRequestResponse
            {
                Result = addSuperVipResult
            });
        }

        [HttpPost]
        public IActionResult EditSuperVipRequest([FromBody] EditSuperVipRequest requestModel)
        {
            var editSuperVipResult =
                _playlistService.EditSuperVipRequest(requestModel.Username, requestModel.CommandText);

            return new JsonResult(new EditRequestResponse
            {
                SongRequestText = editSuperVipResult,
                SyntaxError = string.IsNullOrWhiteSpace(editSuperVipResult)
            });
        }

        [HttpPost]
        public IActionResult PromoteRequest([FromBody] PromoteSongRequest promoteSongRequest)
        {
            return new JsonResult(_playlistService.PromoteRequest(promoteSongRequest.Username));
        }

        [HttpGet]
        public IActionResult ClearRequests()
        {
            try
            {
                _playlistService.ClearRockRequests();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
