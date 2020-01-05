using System.Threading.Tasks;
using CoreCodedChatbot.Api.Interfaces.Queries;
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
        private readonly ILogger<DevOpsController> _logger;

        public DevOpsController(
            IGetAllCurrentWorkItemsQuery getAllCurrentWorkItemsQuery,
            IGetWorkItemByIdQuery getWorkItemByIdQuery,
            ILogger<DevOpsController> logger
            )
        {
            _getAllCurrentWorkItemsQuery = getAllCurrentWorkItemsQuery;
            _getWorkItemByIdQuery = getWorkItemByIdQuery;

            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkItemById(int id)
        {
            var workItem = await _getWorkItemByIdQuery.Get(id);

            return Json(workItem);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCurrentWorkItems()
        {
            var workItems = await _getAllCurrentWorkItemsQuery.Get();

            return Json(workItems);
        }
    }
}