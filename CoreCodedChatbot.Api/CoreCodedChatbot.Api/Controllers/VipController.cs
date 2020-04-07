using System;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiContract.RequestModels.Vip;
using CoreCodedChatbot.ApiContract.ResponseModels.Playlist;
using CoreCodedChatbot.ApiContract.ResponseModels.Vip;
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
        private readonly IVipService _vipService;
        private readonly IPlaylistService _playlistService;
        private readonly ILogger<VipController> _logger;

        public VipController(
            IVipService vipService,
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
                var isSuperVipInQueue = _playlistService.IsSuperVipRequestInQueue();

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

        [HttpGet]
        public IActionResult GetGiftedVips(string username)
        {
            try
            {
                var giftedVips = _vipService.GetUsersGiftedVips(username);

                var response = new GetGiftedVipsResponse
                {
                    GiftedVips = giftedVips
                };

                return new JsonResult(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in GetGiftedVips for user: {username}");
                return BadRequest();
            }
        }


        [HttpGet]
        public IActionResult GetUserVipCount(string username)
        {
            try
            {
                var vips = _vipService.GetUserVipCount(username);

                return new JsonResult(new GetUserVipCountResponse
                {
                    Vips = vips
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in GetUserVipCount");
            }

            return BadRequest();
        }

        [HttpPost]
        public IActionResult GiveSubscriptionVips([FromBody] GiveSubscriptionVipsRequest request)
        {
            try
            {
                _vipService.GiveSubscriptionVips(request.Username);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in GiveSubscriptionVips");
            }

            return BadRequest();
        }

        [HttpPost]
        public IActionResult UpdateBitsDropped([FromBody] UpdateTotalBitsDroppedRequest request)
        {
            try
            {
                _vipService.UpdateTotalBits(request.Username, request.TotalBitsDropped);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in UpdateBitsDropped");
            }

            return BadRequest();
        }

        [HttpPost]
        public IActionResult ConvertBytes([FromBody] ConvertVipsRequest request)
        {
            try
            {
                var convertedBytes = _vipService.ConvertBytes(request.Username, request.NumberOfBytes);

                return new JsonResult(new ByteConversionResponse
                {
                    ConvertedBytes = convertedBytes
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in ConvertBytes");
            }

            return BadRequest();
        }

        [HttpPost]
        public IActionResult ConvertAllBytes([FromBody] ConvertAllVipsRequest request)
        {
            try
            {
                var convertedBytes = _vipService.ConvertAllBytes(request.Username);

                return new JsonResult(new ByteConversionResponse
                {
                    ConvertedBytes = convertedBytes
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in ConvertAllBytes");
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult GetUserByteCount(string username)
        {
            try
            {
                var bytes = _vipService.GetUserByteCount(username);

                return new JsonResult(new GetUserByteCountResponse
                {
                    Bytes = bytes
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error in GetUserByteCount");
            }

            return BadRequest();
        }
    }
}
