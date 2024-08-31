using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class SurveyResultRepo : BaseRepoNumericId<Models.SurveyResult>, ISurveyResultRepo
    {
        private readonly IAnswerRepo _answerRepo;
        private readonly IAnswerOptionRepo _answerOptionRepo;
        
        public SurveyResultRepo(OstDbContext dbContext, IAnswerRepo answerRepo, IAnswerOptionRepo answerOptionRepo) : base(dbContext)
        {
            _answerRepo = answerRepo;
            _answerOptionRepo = answerOptionRepo;
        }

        public override async Task<int> Add(SurveyResult entity)
        {
            ICollection<Answer> answers = entity.Answers;

            entity.Answers = [];
            var result = await Table.AddAsync(entity);

            foreach (var ans in answers)
            {
                ans.SurveyResultId = entity.Id;
                if (ans is AnswerMultipleChoice m)
                    ProcessMultipleChoice(m);
                entity.Answers.Add(ans);
            }

            return await SaveChanges();
        }

        private void ProcessMultipleChoice(AnswerMultipleChoice multipleChoice)
        {
            List<AnswerOption> answerOptions = [];
            answerOptions.AddRange(multipleChoice.ChoiceOptions.Select(co => new AnswerOption
            {
                ResultId = multipleChoice.SurveyResultId,
                Answer = multipleChoice,
                ChoiceOption = co,
                ChoiceOptionId = co.Id,
                Number = multipleChoice.QuestionNumber
            }));
            multipleChoice.AnswerOptions = answerOptions;
        }
    }
}
