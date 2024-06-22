using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

public class ChoiceOptionDTO
{
    public string Value { get; set; }
    public int Number { get; set; }

    public ChoiceOptionDTO()
    {
    }
    
    public ChoiceOptionDTO(ChoiceOption choiceOption)
    {
        Value = choiceOption.Value;
        Number = choiceOption.Number;
    }

    public ChoiceOption ToChoiceOption()
    {
        ChoiceOption choiceOption = new ChoiceOption
        {
            Value = Value,
            Number = Number
        };
        return choiceOption;
    }
}