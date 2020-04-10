using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api.Controllers
{
    [Route("CustomChatCommands/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomChatCommandsController : Controller
    {
        private readonly IChatCommandService _chatCommandService;
        private readonly ILogger<CustomChatCommandsController> _logger;

        public CustomChatCommandsController(
            IChatCommandService chatCommandService, 
            ILogger<CustomChatCommandsController> logger)
        {
            _chatCommandService = chatCommandService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetCommandText(GetCommandTextRequest request)
        {
            try
            {
                var commandText = _chatCommandService.GetCommandText(request.Keyword);


            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error encountered when getting custom command text", new {request.Keyword});
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult GetCommandHelpText(GetCommandHelpTextRequest request)
        {
            try
            {
                var commandText = _chatCommandService.GetCommandHelpText(request.Keyword);


            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error encountered when getting custom command text", new {request.Keyword});
            }

            return BadRequest();
        }

        [HttpPost]
        public IActionResult AddCommand(AddCommandRequest request)
        {
            try
            {

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error encountered when adding a custom command", new {request.});
            }
        }
    }
}