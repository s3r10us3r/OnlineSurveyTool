using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class ChoiceOptionRepo : BaseRepo<ChoiceOption>, IChoiceOptionRepo
    {
        public ChoiceOptionRepo(OSTDbContext dbContext) : base(dbContext)
        {
        }
    }
}
