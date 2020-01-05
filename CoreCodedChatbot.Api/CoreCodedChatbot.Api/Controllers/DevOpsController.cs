using System.Threading.Tasks;
using CoreCodedChatbot.Api.Interfaces.Queries;
using CoreCodedChatbot.ApiContract.RequestModels.DevOps;
using CoreCodedChatbot.ApiContract.ResponseModels.DevOps;
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
        private readonly IGetWorkItemByIdQuery _getWorkItemByIdQuery;
        private readonly IRaiseBugQuery _raiseBugQuery;
        private readonly ILogger<DevOpsController> _logger;

        public DevOpsController(
            IGetAllCurrentWorkItemsQuery getAllCurrentWorkItemsQuery,
            IGetWorkItemByIdQuery getWorkItemByIdQuery,
            IRaiseBugQuery raiseBugQuery,
            ILogger<DevOpsController> logger
            )
        {
            _getAllCurrentWorkItemsQuery = getAllCurrentWorkItemsQuery;
            _getWorkItemByIdQuery = getWorkItemByIdQuery;
            _raiseBugQuery = raiseBugQuery;

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
            return Json(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCurrentWorkItems()
        {
            var workItems = await _getAllCurrentWorkItemsQuery.Get();

            return Json(workItems);
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

            return Json(success);
        }
    }
}