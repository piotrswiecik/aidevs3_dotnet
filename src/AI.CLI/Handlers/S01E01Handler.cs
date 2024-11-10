using AI.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AI.CLI.Handlers;

public class S01E01Handler(IServiceProvider serviceProvider)
{
    private IServiceProvider ServiceProvider { get; init; } = serviceProvider;
    private IPromptService PromptService { get; init; } = serviceProvider.GetRequiredService<IPromptService>();

    public async Task HandleAsync()
    {
        Console.WriteLine("S01E01 running...");
        
        var config = ServiceProvider.GetRequiredService<IConfiguration>();
        
        // get the website
        var siteContent = await GetWebsiteContent(config["Ag3nts:XUrl"] ?? throw new Exception("No URL provided"));
        
        // extract the question using LLM prompt
        var question = await ExtractQuestion(siteContent);
        Console.WriteLine($"Question: {question}");
        
        // answer the question using LLM prompt
        var answer = await AnswerQuestion(question);
        Console.WriteLine($"Answer: {answer}");
        
        // send the login form
        await SendForLogin(config["Ag3nts:XUrl"] ?? throw new Exception("No URL provided"), answer);
        
    }

    private async Task<string> GetWebsiteContent(string url)
    {
        using HttpClient httpClient = new();
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    private async Task SendForLogin(string url, string answer)
    {
        using HttpClient httpClient = new();
        var formContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("username", "tester"),
            new KeyValuePair<string, string>("password", "574e112a"),
            new KeyValuePair<string, string>("answer", answer)
        });
        var response = await httpClient.PostAsync(url, formContent);
        response.EnsureSuccessStatusCode();
        Console.WriteLine("Login successful!");
        Console.WriteLine(response.Content.ReadAsStringAsync().Result);
    }
    
    private async Task<string> ExtractQuestion(string content)
    {
        const string systemPrompt = """
                                     Parse the provided website content and extract a question.
                                     There is only one question in the content.
                                     The question should be a single sentence.
                                     If there is no question, return an empty string.
                                     """;
        var userPrompt = $"Website content: {content}";
        
        // TODO: implement this logic via Semantic Kernel
        return await PromptService.CompletePromptAsync(userPrompt, systemPrompt);
    }

    private async Task<string> AnswerQuestion(string question)
    {
        const string systemPrompt = """
                                     Answer the question. The answer is a single integer number.
                                     Provide the answer only, without any other text, numbers, or characters.
                                     If you cannot answer the question, return 0.
                                     """;
        var userPrompt = $"Question: {question}";
        
        // TODO: implement this logic via Semantic Kernel
        return await PromptService.CompletePromptAsync(userPrompt, systemPrompt);
    }
}