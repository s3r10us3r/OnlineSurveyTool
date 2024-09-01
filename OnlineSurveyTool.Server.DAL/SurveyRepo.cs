using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class SurveyRepo : BaseRepoStringId<Survey>, ISurveyRepo
    {
        public SurveyRepo(OstDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Survey?> GetOne(string id)
        {
            var survey = await Table.Include(s => s.Questions).FirstOrDefaultAsync(s => s.Id == id);
            
            if (survey is null)
                return survey;

            foreach (var q in survey.Questions)
            {
                if (q.Type is QuestionType.MultipleChoice or QuestionType.SingleChoice)
                    await Context.Entry(q).Collection(qs => qs.ChoiceOptions!).LoadAsync();
            }

            return survey;
        }

        public async Task LoadResults(Survey survey)
        {
            await Context.Entry(survey).Collection(s => s.Results).LoadAsync();
        }
    }
}
