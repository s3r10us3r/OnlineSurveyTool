using Microsoft.EntityFrameworkCore.Storage;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Test.Utils.Populators;

namespace OnlineSurveyTool.Test.Utils.Mocks;

public class UnitOfWorkMock : IUnitOfWork
{
    private IUserRepo? _userRepo;
    private IQuestionRepo? _questionRepo;
    private ISurveyRepo? _surveyRepo;
    private IChoiceOptionRepo? _choiceOptionRepo;
    
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public IAnswerRepo AnswerRepo { get; }

    public IAnswerOptionRepo AnswerOptionRepo { get; }
    public IChoiceOptionRepo ChoiceOptionRepo => _choiceOptionRepo ??= new ChoiceOptionRepoMock(new ChoiceOptionPopulator());
    public IQuestionRepo QuestionRepo => _questionRepo ??= new QuestionRepoMock(new QuestionPopulator());
    public ISurveyRepo SurveyRepo => _surveyRepo ??= new SurveyRepoMock(new SurveyPopulator());
    public ISurveyResultRepo SurveyResultRepo { get; }
    public IUserRepo UserRepo => _userRepo ??= new UserRepoMock(new UserPopulator());
    public async Task<int> Save()
    {
        return 1;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return new MockTransaction();
    }

    public async Task CommitTransactionAsync()
    {
    }

    public async Task RollbackTransactionAsync()
    {
    }
}