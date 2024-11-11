using System.Threading.RateLimiting;
using AI.External;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.AspNetCore.RateLimiting;
using Serilog;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables("AI_");

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenAIChatCompletion(
    modelId: "gpt-4o-mini", apiKey: builder.Configuration.GetSection("OpenAI:ApiKey").Value ?? throw new Exception());
builder.Services.AddTransient<Kernel>();

// rate limiter for OpenAI
builder.Services.AddRateLimiter(_ => _.AddFixedWindowLimiter(policyName: "OpenAI.Fixed", options =>
{
    options.PermitLimit = 5;
    options.Window = TimeSpan.FromSeconds(30);
    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    options.QueueLimit = 5;
}));

// Serilog
builder.Host.UseSerilog((ctx, config) =>
{
    config.ReadFrom.Configuration(ctx.Configuration);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseBasicAuth(); // Custom middleware for HTTP Basic Auth
app.UseRateLimiter();
app.UseSerilogRequestLogging();

    
// Using minimal APIs
app.MapPost("/chat", async ([FromBody] ChatRequest request, IServiceProvider provider, ILogger logger) =>
    {
        logger.Information("Chat request: {Message}", request.Message);
        var kernel = provider.GetRequiredService<Kernel>();
        var client = kernel.GetRequiredService<IChatCompletionService>();

        return "test";
        // var res = await client.GetChatMessageContentsAsync(
        //     [new ChatMessageContent(AuthorRole.User, request.Message)]);
        // return new ChatResponse() { Messages = res.Select(x => x.Content).ToArray() };
    })
    .RequireRateLimiting("OpenAI.Fixed")
    .WithName("Chat with LLM")
    .WithOpenApi();

app.Run();

internal record ChatRequest(string Message, string ConversationId);

internal record ChatResponse
{
    public required string?[] Messages { get; init; }

}


