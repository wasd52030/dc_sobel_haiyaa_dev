using FluentScheduler;
using Microsoft.Extensions.Configuration;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// reference -> https://blog.darkthread.net/blog/appsetting-fallback-in-console-app/
IConfiguration configuration = new ConfigurationBuilder()
                                   .AddEnvironmentVariables()
                                   .SetBasePath(Directory.GetCurrentDirectory())
                                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                   .Build();


Configure config = new()
{
    ApiKey = configuration.GetValue<string>("ApiKey")!,
    Channel_sobel_haiyaa_dev__general = configuration.GetValue<string>("Id_discord_sobel_haiyaa_dev_general")!
};

JobManager.Initialize();

// Console.WriteLine(config);
await new DCBot(config).MainAsync();