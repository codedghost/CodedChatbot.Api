using System;
using CoreCodedChatbot.ApiContract.RequestModels.Vip;
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
        private readonly ILogger<VipController> _logger;

        public VipController(IVipService vipService, ILogger<VipController> logger)
        {
            _vipService = vipService;
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
                Console.Out.Write($"{e} - {e.InnerException}");
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
                Console.Error.Write($"ModGiveVIP\nException:\n{e}\nInner:\n{e.InnerException}");
            }

            return BadRequest();
        }
    }
}
