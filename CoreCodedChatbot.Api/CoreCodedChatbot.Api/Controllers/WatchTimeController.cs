using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.ResponseModels.WatchTime;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodedChatbot.Api.Controllers
{
    [Route("WatchTime/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WatchTimeController : Controller
    {
        private readonly IWatchTimeService _watchTimeService;

        public WatchTimeController(IWatchTimeService watchTimeService)
        {
            _watchTimeService = watchTimeService;
        }

        [HttpGet]
        public async Task<GetWatchTimeResponse> GetWatchTime(string username)
        {
            var watchTime = await _watchTimeService.GetWatchTime(username);

            return new GetWatchTimeResponse
            {
                WatchTime = watchTime
            };
        }
    }
}
