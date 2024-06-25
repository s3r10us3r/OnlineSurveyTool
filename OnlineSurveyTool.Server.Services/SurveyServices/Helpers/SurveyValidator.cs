using System.Reflection.Metadata;
using Microsoft.Extensions.Logging;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyServices.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers.Interfaces;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Helpers;

public class SurveyValidator : ISurveyValidator
{
    public bool ValidateSurvey(SurveyDTO surveyDto, out string message)
    {
        if (surveyDto.ClosingDate is not null && surveyDto.OpeningDate is not null)
        {
            if (surveyDto.ClosingDate < surveyDto.OpeningDate)
            {
                message = "Survey closing date is lower tha opening date.";
                return false;
            }
        }

        foreach (var question in surveyDto.Questions)
        {
            if (!ValidateQuestion(question, out message))
            {
                return false;
            }
        }

        if (!ValidateQuestionNumbers(surveyDto.Questions))
        {
            message = "Question numbers violation.";
            return false;
        }
        
        message = "";
        return true;
    }

    public bool ValidateQuestion(QuestionBase questionBase, out string message)
    {
        return questionBase switch
        {
            MultipleChoiceQuestionDTO multipleChoiceQuestionDto => ValidateMultipleChoiceQuestion(multipleChoiceQuestionDto, out message),
            NumericalDoubleQuestionDTO numericalDoubleQuestionDto => ValidateNumericalDoubleQuestion(numericalDoubleQuestionDto, out message),
            NumericalIntegerQuestionDTO numericalIntegerQuestionDto => ValidateNumericalIntegerQuestion(numericalIntegerQuestionDto, out message),
            SingleChoiceQuestionDTO singleChoiceQuestionDto => ValidateSingleChoiceQuestion(singleChoiceQuestionDto, out message),
            TextualQuestionDTO textualQuestionDto => ValidateTextualQuestionDto(textualQuestionDto, out message),
            _ => throw new ArgumentOutOfRangeException(nameof(questionBase))
        };
    }

    private bool ValidateMultipleChoiceQuestion(MultipleChoiceQuestionDTO dto, out string message)
    {
        if (dto.MaximalChoices < dto.MinimalChoices)
        {
            message = "MaximalChoices must be higher than MinimalChoices for a MultipleChoiceQuestion";
            return false;
        }
        if (dto.MaximalChoices > dto.ChoiceOptions.Count)
        {
            message = "MaximalChoices cannot be higher than the number of answers!";
            return false;
        }
        if (dto.ChoiceOptions.Count == 0)
        {
            message = "There must at least one answer!";
            return false;
        }

        if (!ValidateChoiceOptionNumbers(dto.ChoiceOptions))
        {
            message = "ChoiceOptions number violation!";
            return false;
        }
        
        message = "";
        return true;
    }

    private bool ValidateSingleChoiceQuestion(SingleChoiceQuestionDTO dto, out string message)
    {
        if (dto.ChoiceOptions.Count == 0)
        {
            message = "There must be at least one ChoiceOption.";
            return false;
        }

        if (!ValidateChoiceOptionNumbers(dto.ChoiceOptions))
        {
            message = "ChoiceOption number violation!";
            return false;
        }

        message = "";
        return true;
    }

    private bool ValidateNumericalDoubleQuestion(NumericalDoubleQuestionDTO dto, out string message)
    {
        if (dto.MinimalAnswer > dto.MaximalAnswer)
        {
            message = "Minimal answer must be smaller than the maximal answer.";
            return false;
        }

        message = "";
        return true;
    }

    private bool ValidateNumericalIntegerQuestion(NumericalIntegerQuestionDTO dto, out string message)
    {
        if (dto.MinimalAnswer > dto.MaximalAnswer)
        {
            message = "Minimal answer must be smaller than the maximal answer.";
            return false;
        }

        message = "";
        return true;
    }

    private bool ValidateTextualQuestionDto(TextualQuestionDTO dto, out string message)
    {
        if (dto.MinimalLength > dto.MaximalLength)
        {
            message = "Maximal answer length must be higher than minimal answer length.";
            return false;
        }

        message = "";
        return true;
    }
    
    private bool ValidateChoiceOptionNumbers(List<ChoiceOptionDTO> choiceOptions)
    {
        var numberSet = choiceOptions.Select(c => c.Number).ToHashSet();
        return numberSet.Count == choiceOptions.Count;
    }

    private bool ValidateQuestionNumbers(List<QuestionBase> questions)
    {
        var numberSet = questions.Select(c => c.Number).ToHashSet();
        var minNumber = questions.Select(c => c.Number).Min();
        var maxNumber = questions.Select(c => c.Number).Max();
        return numberSet.Count == questions.Count && questions.Count == maxNumber && minNumber == 1;
    }
    
}