using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.RequestModels.ClientId;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api.Controllers;

[Microsoft.AspNetCore.Components.Route("ClientId/[action]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ClientIdController : Controller
{
    public readonly ILogger<ClientIdController> _logger;
    private readonly IClientIdService _clientIdService;

    public ClientIdController(ILogger<ClientIdController> logger,
        IClientIdService clientIdService)
    {
        _logger = logger;
        _clientIdService = clientIdService;
    }

    [HttpPost]
    public async Task<IActionResult> ClientId([FromBody] SetClientIdRequestModel request)
    {
        try
        {
            await _clientIdService.SaveClientId(request.HubType, request.ClientId, request.Username);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error encountered when saving clientId", request.HubType, request.ClientId, request.Username);
        }

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> ClientId([FromQuery]string hubType, [FromQuery]string clientId)
    {
        try
        {
            await _clientIdService.RemoveClientId(hubType, clientId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error encountered when removing clientId", hubType, clientId);
        }

        return Ok();
    }
}