using System;
using CoreCodedChatbot.ApiContract.RequestModels.StreamStatus;
using CoreCodedChatbot.ApiContract.ResponseModels.StreamStatus;
using CoreCodedChatbot.Library.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Rest;

namespace CoreCodedChatbot.Api.Controllers
{
    [Route("StreamStatus/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StreamStatusController : Controller
    {
        private readonly IStreamStatusService _streamStatusService;

        public StreamStatusController(IStreamStatusService streamStatusService)
        {
            _streamStatusService = streamStatusService;
        }

        [HttpGet]
        public IActionResult GetStreamStatus(string broadcasterUsername)
        {
            var isStreamOnline = _streamStatusService.GetStreamStatus(broadcasterUsername);

            return new JsonResult(new GetStreamStatusResponse
            {
                IsOnline = isStreamOnline
            });
        }

        [HttpPut]
        public IActionResult PutStreamStatus(PutStreamStatusRequest streamStatusRequest)
        {
            try
            {
                if (_streamStatusService.SaveStreamStatus(streamStatusRequest))
                    return Ok();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Could not save stream status. Exception: {e} - {e.InnerException}");
            }

            return BadRequest();
        }
    }
}