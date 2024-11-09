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
        SystemChatMessage systemChatMessage = new(systemPrompt ?? "");
        UserChatMessage userChatMessage = new(userPrompt);
        var response = await Client.CompleteChatAsync([systemChatMessage, userChatMessage]);
        return response.Value.Content[0].Text;
    }
    
    public async Task<string> CompletePromptAsync(string userPrompt)
    {
        return await CompletePromptAsync(userPrompt, null);
    }
}