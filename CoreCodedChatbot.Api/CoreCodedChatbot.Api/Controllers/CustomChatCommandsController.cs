using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api.Controllers
{
    [Route("CustomChatCommands/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomChatCommandsController : Controller
    {
        private readonly ILogger<CustomChatCommandsController> _logger;

        public CustomChatCommandsController(ILogger<CustomChatCommandsController> logger)
        {
            _logger = logger;
        }
    }
}