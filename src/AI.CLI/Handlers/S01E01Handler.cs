using AI.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AI.CLI.Handlers;

public class S01E01Handler
{
    private readonly IServiceProvider _serviceProvider;

    public S01E01Handler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task HandleAsync()
    {
        Console.WriteLine("S01E01 running...");
        var client = _serviceProvider.GetRequiredService<IPromptService>();
        var response = await client.CompletePromptAsync("test");
        Console.WriteLine(response);
    }
}