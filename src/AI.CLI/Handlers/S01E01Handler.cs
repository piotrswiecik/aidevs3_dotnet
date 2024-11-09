using AI.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AI.CLI.Handlers;

public class S01E01Handler
{
    private IServiceProvider ServiceProvider { get; init; }
    private IPromptService PromptService { get; init; }
    
    public S01E01Handler(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        PromptService = serviceProvider.GetRequiredService<IPromptService>();
    }
    
    public async Task HandleAsync()
    {
        Console.WriteLine("S01E01 running...");
        
        var config = ServiceProvider.GetRequiredService<IConfiguration>();
        
        // get the website
        var siteContent = await GetWebsiteContent(config["Ag3nts:XUrl"] ?? throw new Exception("No URL provided"));
        
        // extract the question using LLM prompt
        var question = await ExtractQuestion(siteContent);
        Console.WriteLine($"Question: {question}");

    }

    private async Task<string> GetWebsiteContent(string url)
    {
        using HttpClient httpClient = new();
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
    
    private async Task<string> ExtractQuestion(string content)
    {
        const string systemPrompt = $"""
                                     Parse the provided website content and extract a question.
                                     There is only one question in the content.
                                     The question should be a single sentence.
                                     If there is no question, return an empty string.
                                     """;
        var userPrompt = $"Website content: {content}";
        return await PromptService.CompletePromptAsync(userPrompt, systemPrompt);
    }
}