using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL;

public class AnswerNumericalRepo : BaseRepoAnswer<AnswerNumerical>, IAnswerNumericalRepo
{
    public AnswerNumericalRepo(OstDbContext dbContext) : base(dbContext)
    {
    }
}