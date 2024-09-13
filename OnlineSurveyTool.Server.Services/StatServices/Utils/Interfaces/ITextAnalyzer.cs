namespace OnlineSurveyTool.Server.Services.StatServices.Utils.Interfaces;

public interface ITextAnalyzer
{
    Task<Dictionary<string, int>> AnalyzeTexts(ICollection<string> texts);
}


