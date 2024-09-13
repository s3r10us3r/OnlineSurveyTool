using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL.Interfaces
{
    public interface ISurveyResultRepo : IRepoStringId<SurveyResult>
    {
        Task<SurveyResult> LoadAnswers(SurveyResult surveyResult);
    }
}
