using System.Collections.Generic;
using System.Security.Policy;
using System.Threading.Tasks;
using CoreCodedChatbot.ApiApplication.Interfaces.Factories;
using CoreCodedChatbot.Config;
using PrintfulLib.Interfaces.ExternalClients;
using PrintfulLib.Models.ApiRequest;
using PrintfulLib.Models.WebhookResponses;

namespace CoreCodedChatbot.ApiApplication.Factories
{
    public class PrintfulWebhookSetupFactory : IPrintfulWebhookSetupFactory
    {
        private readonly IPrintfulClient _printfulClient;
        private readonly IConfigService _configService;

        public PrintfulWebhookSetupFactory(
            IPrintfulClient printfulClient, 
            IConfigService configService)
        {
            _printfulClient = printfulClient;
            _configService = configService;
        }

        public async Task SetupPrintfulWebhook()
        {
            var currentSettings = await _printfulClient.GetWebhookConfiguration();

            // Only try to setup if webhook is not configured
            if (string.IsNullOrWhiteSpace(currentSettings?.WebhookInfo?.WebhookReturnUrl))
            {
                var request = new SetUpWebhookConfigurationRequest
                {
                    WebhookReturnUrl = $"{_configService.Get<string>("WebsiteLink")}/MerchWebhook/Event",
                    EnabledWebhookEvents = new List<string>
                    {
                        WebhookEventType.PackageShipped.ToWebhookTypeString(),
                        WebhookEventType.PackageReturned.ToWebhookTypeString(),
                        WebhookEventType.OrderFailed.ToWebhookTypeString(),
                        WebhookEventType.OrderCancelled.ToWebhookTypeString(),
                        WebhookEventType.ProductSynced.ToWebhookTypeString(),
                        WebhookEventType.ProductUpdated.ToWebhookTypeString(),
                        WebhookEventType.OrderPutOnHold.ToWebhookTypeString(),
                        WebhookEventType.OrderRemoveHold.ToWebhookTypeString()
                    },
                    OptionalParams = new object{}
                };

                var result = await _printfulClient.SetWebhookConfiguration(request);
            }

        }
    }
}