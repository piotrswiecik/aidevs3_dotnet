using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace AI.KernelTest;

class Program
{
    [Experimental("SKEXP0010")] // suppress warning for experimental openai client
    static async Task Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationManager()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.development.json")
            .Build();
        
        var modelId = "gpt-4o-mini";
        var endpoint = new Uri("https://api.openai.com/v1");
        var apiKey = configuration["OpenAI:ApiKey"]!;

        var builder = Microsoft.SemanticKernel.Kernel.CreateBuilder().AddOpenAIChatCompletion(modelId: modelId, endpoint: endpoint, apiKey: apiKey);
        builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));
        var kernel = builder.Build();
        var kernelService = kernel.Services.GetRequiredService<IChatCompletionService>();
        
        OpenAIPromptExecutionSettings openAiPromptExecutionSettings = new()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };
        
        var history = new ChatHistory();
        
        string? userInput;
        
        do {
            // Collect user input
            Console.Write("User > ");
            userInput = Console.ReadLine();

            // Add user input
            history.AddUserMessage(userInput!);

            // Get the response from the AI
            var result = await kernelService.GetChatMessageContentAsync(
                history,
                executionSettings: openAiPromptExecutionSettings,
                kernel: kernel);

            // Print the results
            Console.WriteLine("Assistant > " + result);

            // Add the message from the agent to the chat history
            history.AddMessage(result.Role, result.Content ?? string.Empty);
        } while (userInput is not null);
    }
}