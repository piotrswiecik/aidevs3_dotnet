using AI.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using AI.Core.Services;
using AI.CLI.Handlers;

namespace AI.CLI;

class Program
{
    static async Task Main(string[] args)
    {
        IServiceCollection services = new ServiceCollection();
        ConfigureServices(services);
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        
        
        var rootCmd = new RootCommand("ai-devs3");
        rootCmd.SetHandler(() =>
        {
            Console.WriteLine("test");
        });

        var commandS01E01 = new Command("s01e01");
        commandS01E01.SetHandler(async () =>
        {
            var handler = new S01E01Handler(serviceProvider);
            await handler.HandleAsync();
        });
        
        var commandS01E02 = new Command("s01e02");
        commandS01E02.SetHandler(() => {});
        
        var commandS01E03 = new Command("s01e03");
        commandS01E03.SetHandler(() => {});
        
        rootCmd.Add(commandS01E01);
        rootCmd.Add(commandS01E02);
        rootCmd.Add(commandS01E03);
        
        await rootCmd.InvokeAsync(args);
    }
    
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IPromptService, OpenAiPromptService>();
        services.AddSingleton<IConfiguration>(BuildConfiguration());
    }
    
    private static IConfigurationRoot BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.development.json")
            .AddEnvironmentVariables("AI_CLI_") // AI_CLI_OPENAI_API_KEY and so on
            .Build();
    }
}