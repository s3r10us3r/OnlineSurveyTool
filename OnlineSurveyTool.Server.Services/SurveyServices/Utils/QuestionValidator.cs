using Microsoft.Extensions.Configuration;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyService.DTOs;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Utils;

public class QuestionValidator : IQuestionValidator
{
    private readonly IConfiguration _configuration;

    public QuestionValidator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool ValidateQuestions(List<QuestionDTO> questions, out string message)
    {
        message = "";
        for (int i = 0; i < questions.Count; i++)
        {
            if (questions[i].Number != i)
            {
                message = "Question number violation.";
                return false;
            }

            if (!ValidateQuestion(questions[i], out var qMessage))
            {
                message = $"Invalid question {i}: {qMessage}";
                return false;
            }
        }

        return true;
    }
    
    public bool ValidateQuestion(QuestionDTO question, out string message)
    {
        message = "";
        var type = StringToQuestionType(question.Type);
        
        if (type is null)
        {
            message = $"Type {question.Type} does not exist.";
            return false;
        }

        return type switch
        {
            QuestionType.SingleChoice => ValidateSingleChoice(question, out message),
            QuestionType.MultipleChoice => ValidateMultipleChoice(question, out message),
            QuestionType.NumericalInteger => ValidateNumericalInteger(question, out message),
            QuestionType.NumericalDouble => ValidateNumericalDouble(question, out message),
            QuestionType.Textual => ValidateTextual(question, out message),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private bool ValidateSingleChoice(QuestionDTO question, out string message)
    {
        if (question.Maximum is not null || question.Minimum is not null)
        {
            message = "Maximum and minimum must be null for Single Choice question type.";
            return false;
        }

        if (question.ChoiceOptions is null || question.ChoiceOptions.Count == 0)
        {
            message = "Choice options must not be empty.";
        }

        return ValidateQuestionChoiceList(question.ChoiceOptions!, out message);
    }

    private bool ValidateMultipleChoice(QuestionDTO question, out string message)
    {
        if (!ValidateMinimumAndMaximum(question.Minimum, question.Maximum, out message))
        {
            return false;
        }
        
        if (question.Maximum % 1 != 0 || question.Minimum % 1 != 0 || question.Minimum < 0)
        {
            message = "Maximum and minimum must be positive integers";
            return false;
        }
        
        if (question.ChoiceOptions is null || question.ChoiceOptions.Count == 0)
        {
            message = "Choice options must not be empty.";
            return false;
        }

        if (question.Maximum > question.ChoiceOptions.Count)
        {
            message = "Maximum must not be higher than the number of options.";
            return false;
        }

        return ValidateQuestionChoiceList(question.ChoiceOptions, out message);
    }

    private bool ValidateNumericalInteger(QuestionDTO question, out string message)
    {
        if (!ValidateMinimumAndMaximum(question.Minimum, question.Maximum, out message))
        {
            return false;
        }

        if (question.Minimum % 1 != 0 || question.Maximum % 1 != 0)
        {
            message = "Minimum and maximum must be integers.";
            return false;
        }

        message = "";
        return true;
    }

    private bool ValidateNumericalDouble(QuestionDTO question, out string message)
    {
        return ValidateMinimumAndMaximum(question.Minimum, question.Maximum, out message);
    }

    private bool ValidateTextual(QuestionDTO question, out string message)
    {
        if (!ValidateMinimumAndMaximum(question.Minimum, question.Maximum, out message))
        {
            return false;
        }

        if (question.Minimum % 1 != 0 || question.Maximum % 1 != 0 || question.Minimum < 0)
        {
            message = "Minimum and maximum must be positive integers.";
            return false;
        }

        var maxLengthString = _configuration["Settings:TextualQuestionMaximumLength"];
        int maxLength = int.Parse(maxLengthString!);

        if (question.Maximum > maxLength)
        {
            message = $"Maximum answer length of a textual question must not be greater than {maxLength}";
            return false;
        }

        message = "";
        return true;
    }
    
    private bool ValidateMinimumAndMaximum(double? minimum, double? maximum, out string message)
    {
        if (minimum is null || maximum is null)
        {
            message = "Maximum and minimum must not be null for this type.";
            return false;
        }

        if (maximum < minimum)
        {
            message = "Maximum must be grater than minimum.";
            return false;
        }

        message = "";
        return true;
    }
    
    private bool ValidateQuestionChoiceList(List<ChoiceOptionDTO> choiceOptions, out string message)
    {
        var sorted = choiceOptions.OrderBy(co => co.Number).ToList();
        for (int i = 0; i < sorted.Count; i++)
        {
            var co = sorted[i];
            if (co.Number != i)
            {
                message = "Choice option number violation.";
                return false;
            }
        }

        message = "";
        return true;
    }
    
    private QuestionType? StringToQuestionType(string type)
    {
        switch (type)
        {
            case "Single Choice":
                return QuestionType.SingleChoice;
            case "Multiple Choice":
                return QuestionType.MultipleChoice;
            case "Numerical Double":
                return QuestionType.NumericalDouble;
            case "Numerical Integer":
                return QuestionType.NumericalInteger;
            case "Textual":
                return QuestionType.Textual;
            default:
                return null;
        }
    }
}