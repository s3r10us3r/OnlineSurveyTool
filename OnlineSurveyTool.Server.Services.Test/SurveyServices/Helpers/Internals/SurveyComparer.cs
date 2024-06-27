using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

namespace OnlineSurveyTool.Server.Services.Test.SurveyServices.Helpers.Internals;

internal class SurveyComparer
{
    public void CompareSurveys(SurveyDTO dto, Survey survey)
    {
        Assert.Multiple(() =>
        {
            Assert.That(dto.ClosingDate, Is.EqualTo(survey.ClosingDate));
            Assert.That(dto.OpeningDate, Is.EqualTo(survey.OpeningDate));
            Assert.That(dto.IsOpen, Is.EqualTo(survey.IsOpen));
            Assert.That(dto.Name, Is.EqualTo(survey.Name));
            Assert.That(dto.Token, Is.EqualTo(survey.Token));

            var sortedDtoQuestions = dto.Questions.OrderBy(q => q.Number);
            var sortedQuestions = survey.Questions.OrderBy(q => q.Number);
            
            var zipped = sortedDtoQuestions.Zip(sortedQuestions, (d, q) => (d, q));
            foreach (var (d, q) in zipped)
            {
                CompareQuestions(d, q);
            }
        });
    }
    
    public void CompareQuestions(QuestionBase questionBase, Question question)
    {
        Assert.Multiple(() =>
        {
            Assert.That(questionBase.Number, Is.EqualTo(question.Number));
            Assert.That(questionBase.CanBeSkipped, Is.EqualTo(question.CanBeSkipped));
            Assert.That(questionBase.Value, Is.EqualTo(question.Value));
        });

        Action<QuestionBase, Question> result = questionBase switch
        {
            MultipleChoiceQuestionDTO => CompareMultipleChoice,
            NumericalDoubleQuestionDTO => CompareNumericalDouble,
            NumericalIntegerQuestionDTO => CompareNumericalInteger,
            SingleChoiceQuestionDTO => CompareSingleChoice,
            TextualQuestionDTO => CompareTextual,
            _ => throw new ArgumentOutOfRangeException(nameof(questionBase))
        };

        result(questionBase, question);
    }

    private void CompareSingleChoice(QuestionBase questionBase, Question question)
    {
        var questionDto = questionBase as SingleChoiceQuestionDTO;
        Assert.That(questionDto, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(questionDto.ChoiceOptions, Has.Count.EqualTo(question.ChoiceOptions!.Count()));
            Assert.That(CompareChoiceOptionLists(questionDto.ChoiceOptions, question.ChoiceOptions!));
        });
    }

    private void CompareMultipleChoice(QuestionBase questionBase, Question question)
    {
        var questionDto = questionBase as MultipleChoiceQuestionDTO;
        Assert.That(questionDto, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(questionDto.ChoiceOptions, Has.Count.EqualTo(question.ChoiceOptions!.Count()));
            Assert.That(CompareChoiceOptionLists(questionDto.ChoiceOptions, question.ChoiceOptions!));
            Assert.That(questionDto.MinimalChoices, Is.EqualTo(question.Minimum));
            Assert.That(questionDto.MaximalChoices, Is.EqualTo(question.Maximum));
        });
    }

    private void CompareNumericalDouble(QuestionBase questionBase, Question question)
    {
        var questionDto = questionBase as NumericalDoubleQuestionDTO;
        Assert.That(questionDto, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(questionDto.MinimalAnswer, Is.EqualTo(question.Minimum));
            Assert.That(questionDto.MaximalAnswer, Is.EqualTo(question.Maximum));
        });
    }
    
    private void CompareNumericalInteger(QuestionBase questionBase, Question question)
    {
        var questionDto = questionBase as NumericalIntegerQuestionDTO;
        Assert.That(questionDto, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(questionDto.MinimalAnswer, Is.EqualTo(question.Minimum));
            Assert.That(questionDto.MaximalAnswer, Is.EqualTo(question.Maximum));
        });
    }
    
    private void CompareTextual(QuestionBase questionBase, Question question)
    {
        var questionDto = questionBase as TextualQuestionDTO;
        Assert.That(questionDto, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(questionDto.MinimalLength, Is.EqualTo(question.Minimum));
            Assert.That(questionDto.MaximalLength, Is.EqualTo(question.Maximum));
        });
    }
    
    private bool CompareChoiceOptionLists(IEnumerable<ChoiceOptionDTO> dtos, IEnumerable<ChoiceOption> choiceOptions)
    {
        dtos = dtos.OrderBy(q => q.Number);
        choiceOptions = choiceOptions.OrderBy(q => q.Number);
        for (int i = 0; i < dtos.Count(); i++)
        {
            var result = CompareChoiceOptions(dtos.ElementAt(i), choiceOptions.ElementAt(i));
            if (!result)
                return false;
        }

        return true;
    }
    
    private bool CompareChoiceOptions(ChoiceOptionDTO dto, ChoiceOption choiceOption)
    {
        return dto.Value == choiceOption.Value && dto.Number == choiceOption.Number;
    }
}