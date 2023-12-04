using CodedChatbot.TwitchFactories;
using CoreCodedChatbot.ApiApplication;
using Microsoft.Extensions.DependencyInjection;

namespace CoreCodedChatbot.Api;

public static class Package
{
    public static IServiceCollection AddTwitchServices(this IServiceCollection services)
    {
        services.AddTwitchFactories();

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddApiServices();

        return services;
    }
}