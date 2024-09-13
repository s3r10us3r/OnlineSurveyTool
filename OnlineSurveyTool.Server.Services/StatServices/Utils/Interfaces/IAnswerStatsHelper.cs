using System.Collections;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.StatServices.DTOs;

namespace OnlineSurveyTool.Server.Services.StatServices.Utils.Interfaces;

public interface IAnswerStatsHelper
{
    Task<AnswerStatistics> GetAnswerStats(Question question, ICollection<SurveyResult> results);
}