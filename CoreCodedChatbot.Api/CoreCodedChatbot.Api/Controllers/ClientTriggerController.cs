using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.RequestModels.ClientTrigger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api.Controllers
{
    [Microsoft.AspNetCore.Components.Route("ClientTrigger/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ClientTriggerController : Controller
    {
        private readonly ILogger<ClientTriggerController> _logger;
        private readonly IClientTriggerService _clientTriggerService;

        public ClientTriggerController(ILogger<ClientTriggerController> logger,
            IClientTriggerService clientTriggerService)
        {
            _logger = logger;
            _clientTriggerService = clientTriggerService;
        }

        [HttpPost]
        public IActionResult CheckBackgroundSong([FromBody] CheckBackgroundSongRequest request)
        {
            try
            {
                _clientTriggerService.TriggerSongCheck(request.Username);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in CheckBackgroundSong");
            }

            return BadRequest();
        }

        [HttpPost]
        public IActionResult SendBackgroundSongResult([FromBody] SendBackgroundSongResultRequest request)
        {
            try
            {
                _clientTriggerService.SendSongToChat(request.Username, request.Title, request.Artist, request.Url);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in SendBackgroundSongResult");
            }

            return BadRequest();
        }
    }
}