using CodedChatbot.TwitchFactories;
using CoreCodedChatbot.ApiApplication;
using CoreCodedChatbot.ApiApplication.Commands;
using CoreCodedChatbot.ApiApplication.Queries;
using CoreCodedChatbot.ApiApplication.Repositories;
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
        services.AddApiServices()
            .AddApiCommands()
            .AddApiQueries();

        return services;
    }
}