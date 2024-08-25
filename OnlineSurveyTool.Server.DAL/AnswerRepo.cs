using System.Runtime.InteropServices.JavaScript;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL;

public class AnswerRepo : IAnswerRepo
{
    private IAnswerSingleChoiceRepo _singleChoiceRepo;
    private IAnswerMultipleChoiceRepo _multipleChoiceRepo;
    private IAnswerNumericalRepo _numericalRepo;
    private IAnswerTextualRepo _textualRepo;
    private ISurveyResultRepo _surveyResultRepo;
    
    public AnswerRepo(IAnswerSingleChoiceRepo singleChoiceRepo, IAnswerMultipleChoiceRepo multipleChoiceRepo,
        IAnswerNumericalRepo numericalRepo, IAnswerTextualRepo textualRepo, ISurveyResultRepo surveyResultRepo)
    {
        _singleChoiceRepo = singleChoiceRepo;
        _multipleChoiceRepo = multipleChoiceRepo;
        _numericalRepo = numericalRepo;
        _textualRepo = textualRepo;
        _surveyResultRepo = surveyResultRepo;
    }
    
    public void Dispose()
    {
        _singleChoiceRepo.Dispose();
        _multipleChoiceRepo.Dispose();
        _numericalRepo.Dispose();
        _textualRepo.Dispose();
    }

    public async Task<int> SaveChanges()
    {
        return await _singleChoiceRepo.SaveChanges() + await _multipleChoiceRepo.SaveChanges() +
               await _numericalRepo.SaveChanges() + await _textualRepo.SaveChanges();
    }

    public async Task<Answer?> GetOne(int resultId, int number)
    {
        return (await _surveyResultRepo.GetOne(resultId))?.Answers.First(a => a.QuestionNumber == number);
    }

    public async Task<int> Remove(int resultId, int number)
    {
        var answer = await GetOne(resultId, number);
        return await Remove(answer);
    }

    public async Task<List<Answer>> GetAll()
    {
        List<Answer> answers = [];
        answers.AddRange(await _singleChoiceRepo.GetAll());
        answers.AddRange(await _multipleChoiceRepo.GetAll());
        answers.AddRange(await _numericalRepo.GetAll());
        answers.AddRange(await _textualRepo.GetAll());
        return answers;
    }

    public async Task<int> Add(Answer entity)
    {
        return await (entity switch
        {
            AnswerMultipleChoice a => _multipleChoiceRepo.Add(a),
            AnswerNumerical a => _numericalRepo.Add(a),
            AnswerSingleChoice a => _singleChoiceRepo.Add(a),
            AnswerTextual a => _textualRepo.Add(a),
            _ => throw new ArgumentOutOfRangeException(nameof(entity))
        });
    }

    public async Task<int> AddRange(IList<Answer> entities)
    {
        List<Task<int>> tasks = [];
        foreach (var e in entities)
        {
            tasks.Add(Add(e));
        }

        return (await Task.WhenAll(tasks)).Sum();
    }

    public async Task<int> Update(Answer entity)
    {
        return await (entity switch
        {
            AnswerMultipleChoice a => _multipleChoiceRepo.Add(a),
            AnswerNumerical a => _numericalRepo.Add(a),
            AnswerSingleChoice a => _singleChoiceRepo.Add(a),
            AnswerTextual a => _textualRepo.Add(a),
            _ => throw new ArgumentOutOfRangeException(nameof(entity))
        });
    }

    public async Task<int> Remove(Answer entity)
    {
        return await (entity switch
        {
            AnswerMultipleChoice a => _multipleChoiceRepo.Add(a),
            AnswerNumerical a => _numericalRepo.Add(a),
            AnswerSingleChoice a => _singleChoiceRepo.Add(a),
            AnswerTextual a => _textualRepo.Add(a),
            _ => throw new ArgumentOutOfRangeException(nameof(entity))
        });
    }
}
