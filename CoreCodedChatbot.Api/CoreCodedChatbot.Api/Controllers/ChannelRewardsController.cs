using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.RequestModels.ChannelRewards;
using CoreCodedChatbot.ApiContract.ResponseModels.ChannelRewards;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api.Controllers
{
    [Route("ChannelRewards/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChannelRewardsController : Controller
    {
        private readonly IChannelRewardsService _channelRewardsService;
        private readonly ILogger<ChannelRewardsController> _logger;

        public ChannelRewardsController(
            IChannelRewardsService channelRewardsService,
                ILogger<ChannelRewardsController> logger
            )
        {
            _channelRewardsService = channelRewardsService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult CreateOrUpdate([FromBody] CreateOrUpdateChannelRewardRequest request)
        {
            try
            {
                _channelRewardsService.CreateOrUpdate(request.ChannelRewardId, request.RewardTitle,
                    request.RewardDescription);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, "Error when creating or updating Channel Reward", request);
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult StoreRedemption([FromBody] StoreRewardRedemptionRequest request)
        {
            try
            {
                var result = _channelRewardsService.Store(request.ChannelRewardId, request.RedeemedBy, Guid.Empty);

                return Ok(new StoreRedemptionResponse
                {
                    CommandType = result
                });
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, "Error when storing a reward redemption", request);
                return BadRequest();
            }
        }
    }
}