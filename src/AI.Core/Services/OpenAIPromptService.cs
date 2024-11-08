using AI.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;

namespace AI.Core.Services;

public class OpenAiPromptService : IPromptService
{
    private ChatClient Client { get; init; } // OpenAI API client
    private IConfiguration Configuration { get; init; } // All available config settings - no need to separate

    public OpenAiPromptService(IConfiguration configuration)
    {
        Configuration = configuration;
        Client = new ChatClient("gpt-4o-mini", Configuration["OpenAI:ApiKey"]);
    }
    
    public async Task<string> CompletePromptAsync(string userPrompt, string? systemPrompt)
    {
        throw new NotImplementedException();
    }
}