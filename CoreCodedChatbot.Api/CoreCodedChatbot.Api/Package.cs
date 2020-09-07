using CodedChatbot.TwitchFactories;
using CoreCodedChatbot.ApiApplication;
using CoreCodedChatbot.Config;
using CoreCodedChatbot.Secrets;
using Microsoft.Extensions.DependencyInjection;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace CoreCodedChatbot.Api
{
    public static class Package
    {
        public static IServiceCollection AddTwitchServices(this IServiceCollection services)
        {
            services.AddTwitchFactories();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddApiServices()
                .AddApiCommands()
                .AddApiQueries()
                .AddApiRepositories();

            return services;
        }
    }
}