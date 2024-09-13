using OnlineSurveyTool.Server.Services.StatServices.DTOs;
using OnlineSurveyTool.Server.Services.StatServices.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.StatServices.Utils;

public class NumAnalyzer : INumAnalyzer
{
    private const int SegmentCount = 100;
    
    public ICollection<SegmentStat> AnalyzeNumbers(ICollection<double> nums, double minimum, double maximum)
    {
        double step = (maximum - minimum) / SegmentCount;
        double i = minimum;
        List<SegmentStat> stats = [];
        while (i <= maximum - step)
        {
            stats.Add(new SegmentStat(i, i + step, nums.Count(n => n >= i && n < i + step)));
            i++;
        }

        return stats;
    }
}