using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.RequestModels.DevOps;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api.Controllers;

[Route("DevOps/[action]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DevOpsController : Controller
{
    private readonly IAzureDevOpsService _azureDevOpsService;
    private readonly ILogger<DevOpsController> _logger;

    public DevOpsController(
        IAzureDevOpsService azureDevOpsService,
        ILogger<DevOpsController> logger
    )
    {
        _azureDevOpsService = azureDevOpsService;

        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkItemById(int id)
    {
        var workItem = await _azureDevOpsService.GetWorkItemById(id);

        DevOpsWorkItem mappedWorkItem;

        if (workItem.WorkItemType() != "Task")
        {
            var workItems = await _azureDevOpsService.GetChildWorkItemsByPbi(workItem);
            
            mappedWorkItem = workItem.ToDevOpsWorkItem(workItems);
        }
        else
        {
            mappedWorkItem = workItem.ToDevOpsTask();
        }

        var response = new GetWorkItemByIdResponse
        {
            DevOpsWorkItem = mappedWorkItem
        };
        return Json(response, GetJsonSerializerSettings.Get());
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCurrentWorkItems()
    {
        var currentPbis = await _azureDevOpsService.GetCommittedPbisForThisIteration();

        var mappedPbis = currentPbis.Select(parentWorkItem =>
        {
            var workItems = _azureDevOpsService.GetChildWorkItemsByPbi(parentWorkItem);

            var mapParentWorkItem = parentWorkItem.ToDevOpsWorkItem(workItems.Result);

            return mapParentWorkItem;
        });
        
        return Json(new GetAllCurrentWorkItemsResponse
        {
            WorkItems = mappedPbis.ToList()
        }, GetJsonSerializerSettings.Get());
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBacklogWorkItems()
    {
        var backlogItems = await _azureDevOpsService.GetBacklogWorkItems();

        var mappedItems = backlogItems.Select(parentWorkItem =>
        {
            var workItems = _azureDevOpsService.GetChildWorkItemsByPbi(parentWorkItem);

            var mapParentWorkItem = parentWorkItem.ToDevOpsWorkItem(workItems.Result);

            return mapParentWorkItem;
        });

        return Json(new GetAllBacklogWorkItemsResponse
        {
            WorkItems = mappedItems.ToList()
        }, GetJsonSerializerSettings.Get());
    }

    [HttpPut]
    public async Task<IActionResult> RaiseBug([FromBody] RaiseBugRequest raiseBugRequest)
    {
        if (raiseBugRequest == null ||
            string.IsNullOrWhiteSpace(raiseBugRequest.TwitchUsername) ||
            raiseBugRequest.BugInfo == null ||
            string.IsNullOrWhiteSpace(raiseBugRequest.BugInfo.Title))
        {
            _logger.LogError("RaiseBugRequest received with an invalid request", raiseBugRequest);

            return BadRequest();
        }

        var success = await _azureDevOpsService.RaiseBugInBacklog(raiseBugRequest.TwitchUsername, raiseBugRequest.BugInfo);

        return Json(success, GetJsonSerializerSettings.Get());
    }

    [HttpPost]
    public async Task<IActionResult> PracticeSongRequest([FromBody] PracticeSongRequest request)
    {
        if (request == null ||
            string.IsNullOrWhiteSpace(request.SongName) ||
            string.IsNullOrWhiteSpace(request.Username)
           )
        {
            _logger.LogError("PracticeSongRequest received an invalid request", request);

            return BadRequest();
        }

        try
        {
            await _azureDevOpsService.RaisePracticeSongRequest(request.Username, new DevOpsProductBacklogItem
            {
                Title = request.SongName,
                Description = request.ExtraInformation,
                Tags = new List<string> { "Song Request"}
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error in PracticeSongRequest when raising pbi in DevOps", request);
            return BadRequest();
        }

        return Ok();
    }

    //[HttpGet]
    //public async Task<IActionResult> GetCurrentBurndownChart()
    //{
    //    var chart = await _azureDevOpsService.GetCurrentSprintBurndownChart();

    //    return new JsonResult(chart);
    //}
}