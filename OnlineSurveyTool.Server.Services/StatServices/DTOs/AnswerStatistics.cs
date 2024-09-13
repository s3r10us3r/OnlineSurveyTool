using System.Text.Json.Serialization;

namespace OnlineSurveyTool.Server.Services.StatServices.DTOs;

[JsonPolymorphic(TypeDiscriminatorPropertyName = nameof(Type))]
[JsonDerivedType(typeof(SingleChoiceAnswerStatistics), "single choice")]
[JsonDerivedType(typeof(MultipleChoiceAnswerStatistics), "multiple choice")]
[JsonDerivedType(typeof(NumericalAnswerStatistics), "numerical")]
[JsonDerivedType(typeof(TextualAnswerStatistics), "textual")]
public abstract record AnswerStatistics
{
    public AnswerStatistics(string type)
    {
        Type = type;
    }
    public string Type { get; private init; }
    public required int Number { get; init; }
    public required string QuestionValue { get; init; }
    public required int NumberOfAnswers { get; init; }
}

//id-count dict
public record SingleChoiceAnswerStatistics() : AnswerStatistics("single choice")
{
    public required ICollection<ChosenOptionStat> ChosenOptionsStats { get; init; } = [];
}

public record ChosenOptionStat(string Value, int Count);

public record MultipleChoiceAnswerStatistics() : AnswerStatistics("multiple choice")
{
    public required ICollection<ChosenOptionsCombinationStat> ChosenOptions { get; init; } = [];
}

public record ChosenOptionsCombinationStat(string[] Values, int Count);

public record NumericalAnswerStatistics() : AnswerStatistics("numerical")
{
    public required double Average { get; init; }
    public required double Median { get; init; }
    public required double Dominant { get; init; }
    public required double Minimum { get; init; }
    public required double Maximum { get; init; }
    public required ICollection<SegmentStat> CountOnSegments { get; init; } = [];
}

public record SegmentStat(double Start, double End, int Count);

public record TextualAnswerStatistics() : AnswerStatistics("textual")
{
    //100 most common words (or less)
    public required IDictionary<string, int> MostCommonWords { get; init; } = new Dictionary<string, int>();
    public required double AverageLength { get; init; }
}
