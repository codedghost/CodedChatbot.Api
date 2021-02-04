using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodedChatbot.MerchContract;
using CodedGhost.RabbitMQTools.Interfaces;
using CodedGhost.RabbitMQTools.Models;
using CoreCodedChatbot.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PrintfulLib.Interfaces.ExternalClients;
using PrintfulLib.Models.ApiRequest;
using PrintfulLib.Models.WebhookResponses;

namespace CoreCodedChatbot.Api.Controllers
{
    [Route("Merch/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MerchController : Controller
    {
        private readonly IRabbitMessagePublisher _rabbitMessagePublisher;
        private readonly IPrintfulClient _printfulClient;
        private readonly IConfigService _configService;
        private readonly ILogger<MerchController> _logger;

        public MerchController(
            IRabbitMessagePublisher rabbitMessagePublisher,
            IPrintfulClient printfulClient,
            IConfigService configService,
            ILogger<MerchController> logger
            )
        {
            _rabbitMessagePublisher = rabbitMessagePublisher;
            _printfulClient = printfulClient;
            _configService = configService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> SetupWebhook()
        {
            var request = new SetUpWebhookConfigurationRequest
            {
                WebhookReturnUrl = $"{_configService.Get<string>("WebsiteLink")}/Merch/WebhookEndpoint",
                EnabledWebhookEvents = new List<string>
                {
                    WebhookEventType.PackageShipped.ToWebhookTypeString()
                }
            };

            var result = await _printfulClient.SetWebhookConfiguration(request);

            return Json(result);
        }

        [HttpPost]
        public IActionResult WebhookEndpoint(PrintfulWebhookResponse response)
        {
            try
            {
                if (response.NumberOfRetries > 1)
                {
                    // adding to proper queue has failed, add to Dead letter queue for this type
                }

                switch (response.EventType)
                {
                    case WebhookEventType.PackageShipped:
                        _rabbitMessagePublisher.Publish(new PackageShippedMessage
                        {
                            EventCreated = response.Created,
                            ShipmentInfo = (ShipmentInfo)response.WebhookData
                        });
                        break;
                    case WebhookEventType.PackageReturned:
                        break;
                    case WebhookEventType.OrderFailed:
                        break;
                    case WebhookEventType.OrderCancelled:
                        break;
                    case WebhookEventType.ProductSynced:
                        break;
                    case WebhookEventType.ProductUpdated:
                        break;
                    case WebhookEventType.StockUpdated:
                        break;
                    case WebhookEventType.OrderPutOnHold:
                        break;
                    case WebhookEventType.OrderRemoveHold:
                        break;
                    case WebhookEventType.NotBound:
                        return BadRequest();
                    default:
                        return BadRequest();
                }
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return BadRequest();
        }
    }
}