using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoreCodedChatbot.Database.Context;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CoreCodedChatbot.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Error.WriteLine($"{Directory.GetCurrentDirectory()}");

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: false)
                .Build();

            Console.Error.WriteLine($"{string.Join('\n', config.AsEnumerable().Select(pair => $"{pair.Key} - {pair.Value}"))}");

            CreateHostBuilder(args, config).Run();
        }

        public static IHost CreateHostBuilder(string[] args, IConfigurationRoot config) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseConfiguration(config);
                    builder.UseStartup<Startup>();
                })
                .Build();
    }
}
