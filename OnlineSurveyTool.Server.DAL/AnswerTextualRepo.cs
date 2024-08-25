using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL;

public class AnswerTextualRepo : BaseRepoAnswer<AnswerTextual>, IAnswerTextualRepo
{
    public AnswerTextualRepo(OstDbContext dbContext) : base(dbContext)
    {
    }
}