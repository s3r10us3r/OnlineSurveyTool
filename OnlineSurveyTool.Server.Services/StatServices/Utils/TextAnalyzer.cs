using OnlineSurveyTool.Server.Services.StatServices.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.StatServices.Utils;

public class TextAnalyzer : ITextAnalyzer
{
    public async Task<Dictionary<string, int>> AnalyzeTexts(ICollection<string> texts)
    {
        return await Task.Run(() =>
        {
            return texts
                .SelectMany(AnalyzeText)
                .GroupBy(e => e.Key)
                .ToDictionary(g => g.Key, g => g.Sum(e => e.Value))
                .OrderByDescending(kvp => kvp.Value)
                .Take(100)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        });
    }

    private Dictionary<string, int> AnalyzeText(string text) => text
        .Trim()
        .Split()
        .Where(t => !string.IsNullOrWhiteSpace(t))
        .GroupBy(w => w)
        .ToDictionary(g => g.Key, g => g.Count());
}