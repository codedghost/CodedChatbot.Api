using System;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using AutoMapper;
using CodedGhost.RabbitMQTools;
using CoreCodedChatbot.ApiApplication.Factories;
using CoreCodedChatbot.ApiApplication.Interfaces.Factories;
using CoreCodedChatbot.ApiApplication.Interfaces.Services;
using CoreCodedChatbot.ApiApplication.Models.Solr;
using CoreCodedChatbot.ApiApplication.Services;
using CoreCodedChatbot.Secrets;
using Microsoft.Extensions.DependencyInjection;
using PrintfulLib.ExternalClients;
using PrintfulLib.Interfaces.ExternalClients;
using SolrNet;

namespace CoreCodedChatbot.ApiApplication
{
    public static class Package
    {
        public static IServiceCollection AddFactories(this IServiceCollection services)
        {
            services.AddSingleton<IPrintfulWebhookSetupFactory, PrintfulWebhookSetupFactory>();

            return services;
        }

        public static IServiceCollection AddSolr(this IServiceCollection services, ISecretService secretService)
        {
            var solrUser = secretService.GetSecret<string>("SolrUsername");
            var solrPass = secretService.GetSecret<string>("SolrPassword");
            var solrUrl = secretService.GetSecret<string>("SolrUrl");

            var credentials = Encoding.ASCII.GetBytes($"{solrUser}:{solrPass}");

            var credentialsBase64 = Convert.ToBase64String(credentials);

            services.AddSolrNet<SongSearch>($"{solrUrl}/SongSearch", options =>
            {
                options.HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", credentialsBase64);
            });

            services.AddScoped<ISolrService, SolrService>();
            services.AddSingleton<IDownloadChartService, DownloadChartService>();

            return services;
        }

        public static IServiceCollection AddRabbitConnectionServices(this IServiceCollection services)
        {
            services.AddConnectionRabbitServices()
                .AddRabbitPublisherService();

            return services;
        }

        public static IServiceCollection AddPrintfulClient(this IServiceCollection services, ISecretService secretService)
        {
            var printfulClient = new PrintfulClient(secretService.GetSecret<string>("PrintfulAPIKey"));

            services.AddSingleton<IPrintfulClient>(printfulClient);

            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
