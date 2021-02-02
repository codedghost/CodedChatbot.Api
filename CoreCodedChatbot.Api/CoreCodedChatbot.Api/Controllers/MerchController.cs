using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api.Controllers
{
    [Route("Merch/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MerchController : Controller
    {
        private readonly ILogger<MerchController> _logger;

        public MerchController(
            ILogger<MerchController> logger
            )
        {
            _logger = logger;
        }


    }
}