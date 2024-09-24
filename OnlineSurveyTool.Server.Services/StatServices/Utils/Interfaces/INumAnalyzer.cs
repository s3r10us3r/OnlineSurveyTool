using System.Collections;
using OnlineSurveyTool.Server.Services.StatServices.DTOs;

namespace OnlineSurveyTool.Server.Services.StatServices.Utils.Interfaces;

public interface INumAnalyzer
{
    ICollection<AnswerCount> AnalyzeNumbers(ICollection<double> nums);
}