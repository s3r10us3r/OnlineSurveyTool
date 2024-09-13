using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL.Interfaces
{
    public interface IAnswerRepo : IBaseAnswerRepo<Answer>
    {
        Task<Answer> LoadAnswerOptions(Answer answer);
    }
}
