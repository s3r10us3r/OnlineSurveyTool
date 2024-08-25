using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL;

public class AnswerSingleChoiceRepo : BaseRepoAnswer<AnswerSingleChoice>, IAnswerSingleChoiceRepo
{
    public AnswerSingleChoiceRepo(OstDbContext dbContext) : base(dbContext)
    {
    }
}