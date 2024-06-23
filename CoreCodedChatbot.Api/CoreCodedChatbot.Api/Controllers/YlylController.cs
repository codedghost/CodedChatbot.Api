using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.RequestModels.Ylyl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api.Controllers
{
    [Route("Ylyl/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class YlylController : Controller
    {
        private readonly IYlylService _ylylService;
        private readonly ILogger<YlylController> _logger;

        public YlylController(
            IYlylService ylylService,
            ILogger<YlylController> logger)
        {
            _ylylService = ylylService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Session([FromBody] YlylSessionRequest request)
        {
            try
            {
                return Ok(await _ylylService.ChangeSession(request));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in Session");
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] YlylSubmissionRequest request)
        {
            try
            {
                await _ylylService.SaveSubmission(request);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in Submit");
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> GetUserSubmissions([FromBody] YlylGetSubmissionsRequest request)
        {
            try
            {
                return Ok(await _ylylService.GetSubmissions(request));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error when getting Users submissions");
            }

            return BadRequest();
        }
    }
}
