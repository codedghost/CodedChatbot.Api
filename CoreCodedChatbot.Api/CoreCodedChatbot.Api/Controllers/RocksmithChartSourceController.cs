using CoreCodedChatbot.ApiApplication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodedChatbot.Api.Controllers
{
    [Route("DevOps/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RocksmithChartSourceController : Controller
    {
        private readonly IRocksmithChartSourceService _rocksmithChartSourceService;

        public RocksmithChartSourceController(
            IRocksmithChartSourceService rocksmithChartSourceService
        )
        {
            _rocksmithChartSourceService = rocksmithChartSourceService;
        }

        [HttpGet]
        public IActionResult Test()
        {
            _rocksmithChartSourceService.Main();

            return Ok();
        }
    }
}