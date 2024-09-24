using System.Collections.Immutable;
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
            ChosenOptionsStats = ConstructSingleChoiceAnswerDict(question, answers),
            IsSkippable = question.CanBeSkipped
        };
    }

    private ICollection<ChosenOptionStat> ConstructSingleChoiceAnswerDict(Question question, ICollection<Answer> answers)
    {
        List<ChosenOptionStat> result = [];
        
        var coIds = answers.Select(a => a.ChoiceOptionId);
        foreach (var co in question.ChoiceOptions)
            result.Add(new ChosenOptionStat(co.Value, answers.Count(a => a.ChoiceOptionId == co.Id)));

        return result;
    }

    private async Task<MultipleChoiceAnswerStatistics> GenMultipleChoiceAnswerStats(Question question, ICollection<Answer> answers)
    {
        await LoadAnswerOptions(answers);
        return new MultipleChoiceAnswerStatistics()
        {
            Number = question.Number,
            QuestionValue = question.Value,
            NumberOfAnswers = answers.Count,
            ChosenOptionCombinationStats = ConstructMultipleCoDict(answers),
            ChosenOptionStats = ConstructMultipleCoCount(answers),
            IsSkippable = question.CanBeSkipped
        };
    }

    private ICollection<ChosenOptionsCombinationStat> ConstructMultipleCoDict(ICollection<Answer> answers)
    {
        var countDict = new Dictionary<StringArray, int>();
        var idsValDict = new Dictionary<string, string>();
        foreach (var ans in answers)
        {
            List<string> coIds = [];
            foreach (var ao in ans.AnswerOptions)
            {
                var co = ao.ChoiceOption;
                idsValDict.TryAdd(co.Id, co.Value);
                coIds.Add(co.Id);
                coIds.Sort();
            }

            var arr = new StringArray(coIds);
            if (!countDict.TryAdd(arr, 1)) {
                countDict[arr] += 1;
            }
        }

        return countDict
            .Select(kvp => new ChosenOptionsCombinationStat(kvp.Key.List.Select(e => idsValDict[e]).ToArray(), kvp.Value))
            .OrderBy(e => e.Count)
            .Take(10)
            .ToList();
    }

    private ICollection<ChosenOptionStat> ConstructMultipleCoCount(ICollection<Answer> answers)
    {
        return answers
            .SelectMany(a => a.AnswerOptions)
            .Select(ao => ao.ChoiceOption)
            .GroupBy(co => (co.Id, co.Value))
            .Select(g => new ChosenOptionStat(g.Key.Value, g.Count()))
            .ToList();
    }

    private async Task LoadAnswerOptions(ICollection<Answer> answers)
    {
        foreach (var a in answers)
            await _answerRepo.LoadAnswerOptions(a);
        var answerOptions = answers.SelectMany(a => a.AnswerOptions);
        foreach (var ao in answerOptions)
            ao.ChoiceOption = (await _choiceOptionRepo.GetOne(ao.ChoiceOptionId))!;
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
            CountOnAnswers = _numAnalyzer.AnalyzeNumbers(nums),
            IsSkippable = question.CanBeSkipped
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
            MostCommonWords = await _textAnalyzer.AnalyzeTexts(texts),
            IsSkippable = question.CanBeSkipped
        };
    }

    private double GetAvgLength(ICollection<string> texts)
    {
        return texts.Average(t => t.Length);
    }

    private class StringArray(List<string> list)
    {
        public ImmutableList<string> List { get; } = list.OrderBy(e => e).ToImmutableList();

        public override int GetHashCode()
        {
            if (List.Count == 0)
                return 0;
            int hash = 17;
            foreach (var str in List)
                hash ^= str.GetHashCode();
            return hash;
        }

        public override bool Equals(object? obj)
        {
            var arr = obj as StringArray;
            if (arr is null)
                return false;
            return arr.List
                .Zip(List)
                .All(pair => pair.First.Equals(pair.Second));
        }
    }
}
