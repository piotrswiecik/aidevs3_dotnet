namespace AI.Core.Interfaces;

/// <summary>
/// Interface for prompting LLM models.
/// </summary>
public interface IPromptService
{
    public Task<string> CompletePromptAsync(string userPrompt, string? systemPrompt);
}