using System;
using CoreCodedChatbot.ApiContract.RequestModels.Vip;
using CoreCodedChatbot.ApiContract.ResponseModels.Vip;
using CoreCodedChatbot.Library.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api.Controllers
{
    [Route("Vip/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VipController : Controller
    {
        private IVipService _vipService;
        private readonly IPlaylistService _playlistService;
        private readonly ILogger<VipController> _logger;

        public VipController(IVipService vipService, 
            IPlaylistService playlistService,
            ILogger<VipController> logger)
        {
            _vipService = vipService;
            _playlistService = playlistService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult GiftVip([FromBody] GiftVipRequest giftVipModel)
        {
            try
            {
                if (_vipService.GiftVip(giftVipModel.DonorUsername, giftVipModel.ReceiverUsername)) return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in GiftVip");
            }

            return BadRequest();
        }

        [HttpPost]
        public IActionResult ModGiveVip([FromBody] ModGiveVipRequest modGiveVipModel)
        {
            try
            {
                if (_vipService.ModGiveVip(modGiveVipModel.ReceivingUsername, modGiveVipModel.VipsToGive)) return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in ModGiveVip");
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult DoesUserHaveVip(string username)
        {
            try
            {
                var hasVip = _vipService.HasVip(username);

                var responseModel = new DoesUserHaveVipResponseModel
                {
                    HasVip = hasVip
                };

                return new JsonResult(responseModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in DoesUserHaveVip", username);
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult DoesUserHaveSuperVip(string username)
        {
            try
            {
                var hasSuperVip = _vipService.HasSuperVip(username);

                var responseModel = new DoesUserHaveSuperVipResponseModel
                {
                    HasSuperVip = hasSuperVip
                };

                return new JsonResult(responseModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in DoesUserHaveSuperVip", username);
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult IsSuperVipInQueue()
        {
            try
            {
                var isSuperVipInQueue = _playlistService.IsSuperRequestInQueue();

                var responseModel = new IsSuperVipInQueueResponse
                {
                    IsInQueue = isSuperVipInQueue
                };

                return new JsonResult(responseModel);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in IsSuperVipInQueue");
                return BadRequest();
            }
        }
    }
}
