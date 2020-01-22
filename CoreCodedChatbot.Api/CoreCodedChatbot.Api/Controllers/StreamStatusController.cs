using System;
using CoreCodedChatbot.Api.Interfaces.Commands;
using CoreCodedChatbot.Api.Interfaces.Queries;
using CoreCodedChatbot.ApiContract.RequestModels.StreamStatus;
using CoreCodedChatbot.ApiContract.ResponseModels.StreamStatus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api.Controllers
{
    [Route("StreamStatus/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StreamStatusController : Controller
    {
        private readonly IGetStreamStatusQuery _getStreamStatusQuery;
        private readonly ISaveStreamStatusCommand _saveStreamStatusCommand;
        private readonly ILogger<StreamStatusController> _logger;

        public StreamStatusController(
            IGetStreamStatusQuery getStreamStatusQuery,
            ISaveStreamStatusCommand saveStreamStatusCommand,
            ILogger<StreamStatusController> logger
            )
        {
            _getStreamStatusQuery = getStreamStatusQuery;
            _saveStreamStatusCommand = saveStreamStatusCommand;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetStreamStatus(string broadcasterUsername)
        {
            var isStreamOnline = _getStreamStatusQuery.Get(broadcasterUsername);

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
                if (_saveStreamStatusCommand.Save(streamStatusRequest))
                    return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in PutStreamStatus");
                Console.Error.WriteLine($"Could not save stream status. Exception: {e} - {e.InnerException}");
            }

            return BadRequest();
        }
    }
}