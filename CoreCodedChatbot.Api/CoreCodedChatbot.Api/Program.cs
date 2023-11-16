using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CoreCodedChatbot.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("hosting.json", optional: true, reloadOnChange: false)
            .Build();

        CreateHostBuilder(args, config).Run();
    }

    public static IHost CreateHostBuilder(string[] args, IConfigurationRoot config) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder =>
            {
                builder.UseUrls(config["server.urls"]);
                builder.PreferHostingUrls(true);
                builder.UseStartup<Startup>();
            })
            .Build();
}