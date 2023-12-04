using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Repositories.StreamStatuses;
using CoreCodedChatbot.ApiContract.RequestModels.StreamStatus;
using CoreCodedChatbot.ApiContract.ResponseModels.StreamStatus;
using CoreCodedChatbot.Database.Context.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api.Controllers;

[Route("StreamStatus/[action]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class StreamStatusController : Controller
{
    private readonly IChatbotContextFactory _chatbotContextFactory;
    private readonly ILogger<StreamStatusController> _logger;

    public StreamStatusController(
        IChatbotContextFactory chatbotContextFactory,
        ILogger<StreamStatusController> logger
    )
    {
        _chatbotContextFactory = chatbotContextFactory;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetStreamStatus(string broadcasterUsername)
    {
        using (var repo = new StreamStatusesRepository(_chatbotContextFactory, _logger))
        {
            return new JsonResult(new GetStreamStatusResponse
            {
                IsOnline = repo.GetStreamStatus(broadcasterUsername)
            });
        }
    }

    [HttpPut]
    public async Task<IActionResult> PutStreamStatus([FromBody] PutStreamStatusRequest streamStatusRequest)
    {
        try
        {
            using (var repo = new StreamStatusesRepository(_chatbotContextFactory, _logger))
            {
                if (await repo.Save(streamStatusRequest))
                    return Ok();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in PutStreamStatus");
            Console.Error.WriteLine($"Could not save stream status. Exception: {e} - {e.InnerException}");
        }

        return BadRequest();
    }
}