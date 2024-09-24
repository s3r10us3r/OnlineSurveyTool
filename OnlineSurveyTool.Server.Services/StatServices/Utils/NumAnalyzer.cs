using OnlineSurveyTool.Server.Services.StatServices.DTOs;
using OnlineSurveyTool.Server.Services.StatServices.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.StatServices.Utils;

public class NumAnalyzer : INumAnalyzer
{
    private const int SegmentCount = 100;

    public ICollection<AnswerCount> AnalyzeNumbers(ICollection<double> nums)
    {
        var dict = new Dictionary<double, int>();
        foreach (var num in nums)
        {
            if (!dict.TryAdd(num, 1))
                dict[num] += 1;
        }

        return dict.Select(kvp => new AnswerCount(kvp.Key, kvp.Value)).ToList();
    }
}