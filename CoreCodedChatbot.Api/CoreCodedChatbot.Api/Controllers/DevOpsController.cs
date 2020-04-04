using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Extensions;
using CoreCodedChatbot.ApiApplication.Interfaces.Queries.AzureDevOps;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.RequestModels.DevOps;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps.ChildModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api.Controllers
{
    [Route("DevOps/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DevOpsController : Controller
    {
        private readonly IGetAllCurrentWorkItemsQuery _getAllCurrentWorkItemsQuery;
        private readonly IGetAllBacklogWorkItemsQuery _getAllBacklogWorkItemsQuery;
        private readonly IGetWorkItemByIdQuery _getWorkItemByIdQuery;
        private readonly IRaiseBugQuery _raiseBugQuery;
        private readonly IAzureDevOpsService _azureDevOpsService;
        private readonly ILogger<DevOpsController> _logger;

        public DevOpsController(
            IGetAllCurrentWorkItemsQuery getAllCurrentWorkItemsQuery,
            IGetAllBacklogWorkItemsQuery getAllBacklogWorkItemsQuery,
            IGetWorkItemByIdQuery getWorkItemByIdQuery,
            IRaiseBugQuery raiseBugQuery,
            IAzureDevOpsService azureDevOpsService,
            ILogger<DevOpsController> logger
            )
        {
            _getAllCurrentWorkItemsQuery = getAllCurrentWorkItemsQuery;
            _getAllBacklogWorkItemsQuery = getAllBacklogWorkItemsQuery;
            _getWorkItemByIdQuery = getWorkItemByIdQuery;
            _raiseBugQuery = raiseBugQuery;
            _azureDevOpsService = azureDevOpsService;

            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkItemById(int id)
        {
            var workItem = await _getWorkItemByIdQuery.Get(id);

            var response = new GetWorkItemByIdResponse
            {
                DevOpsWorkItem = workItem
            };
            return Json(response, GetJsonSerializerSettings.Get());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCurrentWorkItems()
        {
            var workItems = await _getAllCurrentWorkItemsQuery.Get();

            return Json(workItems, GetJsonSerializerSettings.Get());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBacklogWorkItems()
        {
            var workItems = await _getAllBacklogWorkItemsQuery.Get();

            return Json(workItems, GetJsonSerializerSettings.Get());
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

            var success = await _raiseBugQuery.Raise(raiseBugRequest.TwitchUsername, raiseBugRequest.BugInfo);

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
                _azureDevOpsService.RaisePracticeSongRequest(request.Username, new DevOpsProductBacklogItem
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
    }
}