using Microsoft.EntityFrameworkCore.Storage;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IAnswerRepo AnswerRepo { get; }
    IAnswerOptionRepo AnswerOptionRepo { get; }
    IChoiceOptionRepo ChoiceOptionRepo { get; }
    IQuestionRepo QuestionRepo { get; }
    ISurveyRepo SurveyRepo { get; }
    ISurveyResultRepo SurveyResultRepo { get; }
    IUserRepo UserRepo { get; }
    
    Task<int> Save();

    Task<IDbContextTransaction> BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}