using Microsoft.EntityFrameworkCore.Storage;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL;

public class UnitOfWork : IUnitOfWork
{
    private IDbContextTransaction? _currentTransaction;
    
    private readonly OstDbContext _context = new();
    private IAnswerOptionRepo? _answerOptionRepo;
    private IChoiceOptionRepo? _choiceOptionRepo;
    private IQuestionRepo? _questionRepo;
    private ISurveyRepo? _surveyRepo;
    private ISurveyResultRepo? _surveyResultRepo;
    private IUserRepo? _userRepo;
    private IAnswerSingleChoiceRepo? _singleChoiceRepo;
    private IAnswerMultipleChoiceRepo? _multipleChoiceRepo;
    private IAnswerNumericalRepo? _numericalRepo;
    private IAnswerTextualRepo? _textualRepo;
    private IAnswerRepo? _answerRepo;
    
    public IAnswerOptionRepo AnswerOptionRepo => _answerOptionRepo ??= new AnswerOptionRepo(_context);
    public IChoiceOptionRepo ChoiceOptionRepo => _choiceOptionRepo ??= new ChoiceOptionRepo(_context);
    public IQuestionRepo QuestionRepo => _questionRepo ??= new QuestionRepo(_context);
    public ISurveyRepo SurveyRepo => _surveyRepo ??= new SurveyRepo(_context);
    public ISurveyResultRepo SurveyResultRepo => _surveyResultRepo ??= new SurveyResultRepo(_context);
    public IUserRepo UserRepo => _userRepo ??= new UserRepo(_context);
    public IAnswerTextualRepo AnswerTextualRepo => _textualRepo ??= new AnswerTextualRepo(_context);
    public IAnswerSingleChoiceRepo AnswerSingleChoiceRepo => _singleChoiceRepo ??= new AnswerSingleChoiceRepo(_context);

    public IAnswerMultipleChoiceRepo AnswerMultipleChoiceRepo =>
        _multipleChoiceRepo ??= new AnswerMultipleChoiceRepo(_context);

    public IAnswerNumericalRepo AnswerNumericalRepo => _numericalRepo ??= new AnswerNumericalRepo(_context);

    public IAnswerRepo AnswerRepo => _answerRepo ??= new AnswerRepo(AnswerSingleChoiceRepo, AnswerMultipleChoiceRepo,
        AnswerNumericalRepo, AnswerTextualRepo, SurveyResultRepo);
    
    public async Task<int> Save()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            throw new InvalidOperationException("There is already an active transaction.");
        }

        _currentTransaction = await _context.Database.BeginTransactionAsync();
        return _currentTransaction;
    }

    public async Task CommitTransactionAsync()
    {
        if (_currentTransaction is null)
        {
            throw new InvalidOperationException("There is no active transaction.");
        }

        try
        {
            await _context.SaveChangesAsync();
            await _currentTransaction.CommitAsync();
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction is null)
        {
            throw new InvalidOperationException("There is no active transaction.");
        }
        try
        {
            await _currentTransaction.RollbackAsync();
        }
        finally
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
    public void Dispose()
    { 
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}