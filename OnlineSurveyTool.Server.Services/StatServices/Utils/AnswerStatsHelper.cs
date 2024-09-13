using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.StatServices.DTOs;
using OnlineSurveyTool.Server.Services.StatServices.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.StatServices.Utils;

public class AnswerStatsHelper : IAnswerStatsHelper
{
    private readonly ITextAnalyzer _textAnalyzer;
    private readonly INumAnalyzer _numAnalyzer;
    private readonly IAnswerRepo _answerRepo;
    private readonly IChoiceOptionRepo _choiceOptionRepo;
    
    public AnswerStatsHelper(ITextAnalyzer textAnalyzer, INumAnalyzer numAnalyzer, IAnswerRepo answerRepo,
        IChoiceOptionRepo choiceOptionRepo)
    {
        _textAnalyzer = textAnalyzer;
        _numAnalyzer = numAnalyzer;
        _answerRepo = answerRepo;
        _choiceOptionRepo = choiceOptionRepo;
    }

    public async Task<AnswerStatistics> GetAnswerStats(Question question, ICollection<SurveyResult> results)
    {
        var answers = ExtractAnswers(question, results);
        
        return question.Type switch
        {
            QuestionType.SingleChoice =>  GenSingleChoiceAnswerStats(question, answers),
            QuestionType.MultipleChoice => await GenMultipleChoiceAnswerStats(question, answers),
            QuestionType.NumericalInteger => GenNumericalAnswerStats(question, answers),
            QuestionType.NumericalDouble => GenNumericalAnswerStats(question, answers),
            QuestionType.Textual => await GenTextualAnswerStats(question, answers),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private ICollection<Answer> ExtractAnswers(Question question, ICollection<SurveyResult> results)
    {
        return results
            .Select(r => r.Answers.FirstOrDefault(a => a.QuestionNumber == question.Number))
            .Where(a => a is not null)
            .ToList()!;
    }
    
    private SingleChoiceAnswerStatistics GenSingleChoiceAnswerStats(Question question,
        ICollection<Answer> answers)
    {
        return new SingleChoiceAnswerStatistics()
        {
            Number = question.Number,
            QuestionValue = question.Value,
            NumberOfAnswers = answers.Count,
            ChosenOptionsStats = ConstructSingleChoiceAnswerDict(answers)
        };
    }

    private ICollection<ChosenOptionStat> ConstructSingleChoiceAnswerDict(ICollection<Answer> answers)
    {
        var idValDict = new Dictionary<string, string>();
        
        return answers
            .GroupBy(a =>
            {
                var co = a.ChoiceOption;
                idValDict.Add(co!.Id, co.Value);
                return co.Id;
            })
            .ToDictionary(g => g.Key, g => g.Count())
            .Select(kvp => new ChosenOptionStat(idValDict[kvp.Key], kvp.Value))
            .ToList();
    }

    private async Task<MultipleChoiceAnswerStatistics> GenMultipleChoiceAnswerStats(Question question, ICollection<Answer> answers)
    {
        return new MultipleChoiceAnswerStatistics()
        {
            Number = question.Number,
            QuestionValue = question.Value,
            NumberOfAnswers = answers.Count,
            ChosenOptions = await ConstructMultipleCoDict(answers)
        };
    }

    private async Task<ICollection<ChosenOptionsCombinationStat>> ConstructMultipleCoDict(ICollection<Answer> answers)
    {
        var countDict = new Dictionary<string[], int>();
        var idsValDict = new Dictionary<string, string>();
        foreach (var ans in answers)
        {
            await _answerRepo.LoadAnswerOptions(ans);
            List<string> coIds = [];
            foreach (var ao in ans.AnswerOptions)
            {
                var co = await _choiceOptionRepo.GetOne(ao.ChoiceOptionId);
                if (co is null)
                    continue;
                idsValDict.Add(co.Id, co.Value);
                coIds.Add(co.Id);
                coIds.Sort();
            }

            var arr = coIds.ToArray();
            if (!countDict.TryAdd(arr, 0))
                countDict[arr]++;
        }
        return countDict
            .Select(kvp => new ChosenOptionsCombinationStat(kvp.Key.Select(e =>idsValDict[e]).ToArray(), kvp.Value))
            .ToList();
    }

    private NumericalAnswerStatistics GenNumericalAnswerStats(Question question, ICollection<Answer> answers)
    {
        var nums = answers
            .Where(a => a.NumericalAnswer is not null)
            .Select(a => (double)a.NumericalAnswer!)
            .ToList();
        nums.Sort();
        
        return new NumericalAnswerStatistics()
        {
            Number = question.Number,
            QuestionValue = question.Value,
            NumberOfAnswers = answers.Count,
            Average = nums.Average(),
            Median = nums[nums.Count / 2],
            Dominant = FindDominant(nums),
            Minimum = (double)question.Minimum!,
            Maximum = (double)question.Maximum!,
            CountOnSegments = _numAnalyzer.AnalyzeNumbers(nums, (double)question.Minimum, (double)question.Maximum)
        };
    }
    
    private double FindDominant(ICollection<double> nums) => nums
        .GroupBy(n => n)
        .Select(g => new { Key = g.Key, Count = g.Count() })
        .OrderByDescending(g => g.Count)
        .First().Key;
    
    private async Task<TextualAnswerStatistics> GenTextualAnswerStats(Question question, ICollection<Answer> answers)
    {
        var texts = answers.Where(a => a.TextAnswer is not null).Select(a => a.TextAnswer!).ToList();

        return new TextualAnswerStatistics()
        {
            Number = question.Number,
            QuestionValue = question.Value,
            NumberOfAnswers = answers.Count,
            AverageLength = GetAvgLength(texts),
            MostCommonWords = await _textAnalyzer.AnalyzeTexts(texts)
        };
    }

    private double GetAvgLength(ICollection<string> texts)
    {
        return texts.Average(t => t.Length);
    }
}