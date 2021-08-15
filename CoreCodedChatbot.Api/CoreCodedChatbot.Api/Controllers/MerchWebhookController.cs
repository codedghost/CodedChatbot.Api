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
    [Route("MerchWebhook/[action]")]
    public class MerchWebhookController : Controller
    {
        private readonly IRabbitMessagePublisher _rabbitMessagePublisher;
        private readonly ILogger<MerchWebhookController> _logger;

        public MerchWebhookController(
            IRabbitMessagePublisher rabbitMessagePublisher,
            IPrintfulClient printfulClient,
            IConfigService configService,
            ISecretService secretService,
            ILogger<MerchWebhookController> logger
            )
        {
            _rabbitMessagePublisher = rabbitMessagePublisher;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Event([FromBody] PrintfulWebhookResponse response)
        {
            // TODO Re-enable commented code when testing is done
            // As this is an unprotected endpoint, we need to ensure that this is a legitimate request
            //if (response.StoreId != _secretService.GetSecret<int>("PrintfulStoreId"))
            //{
            //    return BadRequest();
            //}

            try
            {
                if (response.NumberOfRetries > 1)
                {
                    // adding to proper queue has failed, add to Dead letter queue for this type
                    //_rabbitMessagePublisher.Publish(new MerchDeadLetter
                    //{
                    //    EventCreated = response.Created,
                    //    DataObjectString = JsonConvert.SerializeObject(response.WebhookDataObject)
                    //});
                }

                switch (response.EventType)
                {
                    case WebhookEventType.PackageShipped:
                        _rabbitMessagePublisher.Publish(new PackageShippedMessage
                        {
                            EventCreated = response.Created,
                            ShipmentInfo = (ShipmentInfo) response.WebhookData
                        });
                        break;
                    case WebhookEventType.PackageReturned:
                        _rabbitMessagePublisher.Publish(new PackageReturnedMessage
                        {
                            EventCreated = response.Created,
                            ReturnInfo = (ReturnInfo) response.WebhookData
                        });
                        break;
                    case WebhookEventType.OrderFailed:
                        _rabbitMessagePublisher.Publish(new OrderFailedMessage
                        {
                            EventCreated = response.Created,
                            OrderStatusChange = (OrderStatusChange) response.WebhookData
                        });
                        break;
                    case WebhookEventType.OrderCancelled:
                        _rabbitMessagePublisher.Publish(new OrderCancelledMessage
                        {
                            EventCreated = response.Created,
                            OrderStatusChange = (OrderStatusChange) response.WebhookData
                        });
                        break;
                    case WebhookEventType.ProductSynced:
                        _rabbitMessagePublisher.Publish(new ProductSyncedMessage
                        {
                            EventCreated = response.Created,
                            SyncInfo = (SyncInfo) response.WebhookData
                        });;
                        break;
                    case WebhookEventType.ProductUpdated:
                        _rabbitMessagePublisher.Publish(new ProductUpdatedMessage
                        {
                            EventCreated = response.Created,
                            SyncInfo = (SyncInfo) response.WebhookData
                        });
                        break;
                    case WebhookEventType.StockUpdated:
                        _rabbitMessagePublisher.Publish(new StockUpdatedMessage
                        {
                            EventCreated = response.Created,
                            ProductStock = (ProductStock) response.WebhookData
                        });
                        break;
                    case WebhookEventType.OrderPutOnHold:
                        _rabbitMessagePublisher.Publish(new OrderPutOnHoldMessage
                        {
                            EventCreated = response.Created,
                            OrderStatusChange = (OrderStatusChange) response.WebhookData
                        });
                        break;
                    case WebhookEventType.OrderRemoveHold:
                        _rabbitMessagePublisher.Publish(new OrderRemoveHoldMessage
                        {
                            EventCreated = response.Created,
                            OrderStatusChange = (OrderStatusChange) response.WebhookData
                        });
                        break;
                    case WebhookEventType.NotBound:
                        _rabbitMessagePublisher.Publish(new MerchDeadLetter
                        {
                            EventCreated = response.Created,
                            DataObjectString = JsonConvert.SerializeObject(response.WebhookDataObject)
                        });
                        break;
                    default:
                        return BadRequest();
                }

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}