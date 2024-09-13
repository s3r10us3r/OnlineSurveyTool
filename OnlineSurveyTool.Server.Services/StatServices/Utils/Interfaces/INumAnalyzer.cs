using System.Collections;
using OnlineSurveyTool.Server.Services.StatServices.DTOs;

namespace OnlineSurveyTool.Server.Services.StatServices.Utils.Interfaces;

public interface INumAnalyzer
{
    ICollection<SegmentStat> AnalyzeNumbers(ICollection<double> nums, double minimum, double maximum);
}