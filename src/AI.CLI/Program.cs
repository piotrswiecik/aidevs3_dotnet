using AI.Core.Interfaces;
using AI.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AI.CLI;

class Program
{
    static void Main(string[] args)
    {
        IServiceCollection services = new ServiceCollection();
        ConfigureServices(services);
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        
        var key = serviceProvider.GetRequiredService<IConfiguration>()["OpenAI:ApiKey"];
        Console.WriteLine(key);
    }
    
    private static void ConfigureServices(IServiceCollection services)
    {
        //services.AddSingleton<IPromptService, OpenAiPromptService>();
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