using CoreCodedChatbot.ApiApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodedChatbot.Api.Controllers
{
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