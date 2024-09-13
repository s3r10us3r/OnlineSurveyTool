using Microsoft.EntityFrameworkCore;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class SurveyResultRepo : BaseRepoStringId<SurveyResult>, ISurveyResultRepo
    {
        public SurveyResultRepo(OstDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<SurveyResult?> GetOne(string id)
        {
            return await Context.SurveyResults
                .Include(sr => sr.Answers)
                .FirstOrDefaultAsync(sr => sr.Id == id);
        }

        public async Task<SurveyResult> LoadAnswers(SurveyResult surveyResult)
        {
            await Context.Entry(surveyResult).Collection(sr => sr.Answers).LoadAsync();
            return surveyResult;
        }
    }
}