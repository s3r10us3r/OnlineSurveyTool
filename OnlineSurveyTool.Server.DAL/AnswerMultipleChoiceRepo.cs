using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL;

public class AnswerMultipleChoiceRepo : BaseRepoAnswer<AnswerMultipleChoice>, IAnswerMultipleChoiceRepo
{
    public AnswerMultipleChoiceRepo(OstDbContext dbContext) : base(dbContext)
    {
    }

}