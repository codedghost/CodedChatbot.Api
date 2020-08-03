using System;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.RequestModels.Moderation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api.Controllers
{
    [Route("Moderation/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ModerationController : Controller
    {
        private readonly IModerationService _moderationService;
        private readonly ILogger<ModerationController> _logger;

        public ModerationController(
            IModerationService moderationService,
            ILogger<ModerationController> logger)
        {
            _moderationService = moderationService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult TransferUserAccount([FromBody]TransferUserAccountRequest request)
        { 
            try
            {
                if (string.IsNullOrWhiteSpace(request.RequestingModerator) ||
                    string.IsNullOrWhiteSpace(request.OldUsername) || string.IsNullOrWhiteSpace(request.NewUsername))
                {
                    return BadRequest();
                }

                _moderationService.TransferUserAccount(request.RequestingModerator, request.OldUsername,
                    request.NewUsername);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error encountered when attempting to Transfer a User Account",
                    new object[] {request.RequestingModerator, request.OldUsername, request.NewUsername});
            }

            return BadRequest();
        }
    }
}