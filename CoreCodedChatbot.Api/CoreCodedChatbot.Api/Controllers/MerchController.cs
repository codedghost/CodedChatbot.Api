using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodedChatbot.MerchContract;
using CodedGhost.RabbitMQTools.Interfaces;
using CodedGhost.RabbitMQTools.Models;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PrintfulLib.Converters;
using PrintfulLib.Interfaces.ExternalClients;
using PrintfulLib.Models.ApiRequest;
using PrintfulLib.Models.WebhookResponses;

namespace CoreCodedChatbot.Api.Controllers
{
    [Route("Merch/[action]")]
    public class MerchController : Controller
    {
        private readonly IRabbitMessagePublisher _rabbitMessagePublisher;
        private readonly IPrintfulClient _printfulClient;
        private readonly IConfigService _configService;
        private readonly ISecretService _secretService;
        private readonly ILogger<MerchController> _logger;

        public MerchController(
            IRabbitMessagePublisher rabbitMessagePublisher,
            IPrintfulClient printfulClient,
            IConfigService configService,
            ISecretService secretService,
            ILogger<MerchController> logger
            )
        {
            _rabbitMessagePublisher = rabbitMessagePublisher;
            _printfulClient = printfulClient;
            _configService = configService;
            _secretService = secretService;
            _logger = logger;
        }

        //[HttpGet]
        //public async Task<IActionResult> SetupWebhook()
        //{
        //    var request = new SetUpWebhookConfigurationRequest
        //    {
        //        WebhookReturnUrl = $"{_configService.Get<string>("WebsiteLink")}/Merch/WebhookEndpoint",
        //        EnabledWebhookEvents = new List<string>
        //        {
        //            WebhookEventType.PackageShipped.ToWebhookTypeString()
        //        }
        //    };

        //    var result = await _printfulClient.SetWebhookConfiguration(request);

        //    return Json(result);
        //}

        public class PrintfulWebhookModel
        {
            [JsonProperty("type")]
            private string Type { get; set; }

            [JsonProperty("created")]
            [JsonConverter(typeof(TimestampDateTimeConverter))]
            public DateTime Created { get; set; }

            [JsonProperty("retries")]
            public int NumberOfRetries { get; set; }

            [JsonProperty("store")]
            public int StoreId { get; set; }

            [JsonProperty("data")]
            public object WebhookData { get; set; }
        }

        [HttpPost]
        public IActionResult WebhookEndpoint([FromBody] PrintfulWebhookModel response)
        {
            // As this is an unprotected endpoint, we need to ensure that this is a legitimate request
            if (response.StoreId != _secretService.GetSecret<int>("PrintfulStoreId"))
            {
                return BadRequest();
            }

            try
            {
                if (response.NumberOfRetries > 1)
                {
                    // adding to proper queue has failed, add to Dead letter queue for this type
                }

                //switch (response.GetType())
                //{
                //    case WebhookEventType.PackageShipped:
                //        _rabbitMessagePublisher.Publish(new PackageShippedMessage
                //        {
                //            EventCreated = response.Created,
                //            ShipmentInfo = (ShipmentInfo)response.WebhookData
                //        });
                //        break;
                //    case WebhookEventType.PackageReturned:
                //        break;
                //    case WebhookEventType.OrderFailed:
                //        break;
                //    case WebhookEventType.OrderCancelled:
                //        break;
                //    case WebhookEventType.ProductSynced:
                //        break;
                //    case WebhookEventType.ProductUpdated:
                //        break;
                //    case WebhookEventType.StockUpdated:
                //        break;
                //    case WebhookEventType.OrderPutOnHold:
                //        break;
                //    case WebhookEventType.OrderRemoveHold:
                //        break;
                //    case WebhookEventType.NotBound:
                //        return BadRequest();
                //    default:
                //        return BadRequest();
                //}
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return BadRequest();
        }
    }
}