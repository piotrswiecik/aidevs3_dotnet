namespace AI.Core.Interfaces;

/// <summary>
/// Interface for prompting LLM models.
/// </summary>
public interface IPromptService
{
    
    /// <summary>
    /// Complete prompt using LLM. Optionally provide system prompt.
    /// </summary>
    /// <param name="userPrompt">Required user prompt.</param>
    /// <param name="systemPrompt">Optional system prompt.</param>
    /// <returns>Response text extracted from LLM API.</returns>
    public Task<string> CompletePromptAsync(string userPrompt, string? systemPrompt);
    
    /// <summary>
    /// Complete prompt using LLM.
    /// </summary>
    /// <param name="userPrompt">Required user prompt.</param>
    /// <returns>Response text extracted from LLM API.</returns>
    public Task<string> CompletePromptAsync(string userPrompt);
}