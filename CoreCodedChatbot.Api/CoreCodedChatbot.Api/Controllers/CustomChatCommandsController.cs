using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.RequestModels.CustomChatCommands;
using CoreCodedChatbot.ApiContract.ResponseModels.CustomChatCommands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api.Controllers;

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
    public IActionResult GetCommandText(string keyword)
    {
        try
        {
            var commandText = _chatCommandService.GetCommandText(keyword);

            return new JsonResult(new GetCommandTextResponse
            {
                CommandText = commandText
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error encountered when getting custom command text", new {keyword});
        }

        return BadRequest();
    }

    [HttpGet]
    public IActionResult GetCommandHelpText(string keyword)
    {
        try
        {
            var helpText = _chatCommandService.GetCommandHelpText(keyword);

            return new JsonResult(new GetCommandHelpTextResponse
            {
                HelpText = helpText
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error encountered when getting custom command text", new {keyword});
        }

        return BadRequest();
    }

    [HttpPost]
    public IActionResult AddCommand([FromBody] AddCommandRequest request)
    {
        try
        {
            _chatCommandService.AddCommand(request.Aliases, request.InformationText, request.HelpText,
                request.Username);

            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error encountered when adding a custom command", new  {request.Aliases, request.InformationText, request.HelpText, request.Username});
        }

        return BadRequest();
    }
}