namespace OnlineSurveyTool.Server.Services.StatServices.DTOs;

public record SurveyResultsStatistics(string SurveyName, int ResultCount, ICollection<AnswerStatistics> AnswerStats);