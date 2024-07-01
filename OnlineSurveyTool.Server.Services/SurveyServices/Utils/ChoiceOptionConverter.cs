using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyService.DTOs;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Utils;

public class ChoiceOptionConverter : IChoiceOptionConverter
{
    private IGuidGenerator _guidGenerator;

    public ChoiceOptionConverter(IGuidGenerator guidGenerator)
    {
        _guidGenerator = guidGenerator;
    }
    
    public ChoiceOption DtoToChoiceOption(ChoiceOptionDTO dto)
    {
        return new ChoiceOption()
        {
            Number = dto.Number,
            Id = dto.Id ?? _guidGenerator.GenerateGuid(),
            Value = dto.Value
        };
    }

    public ChoiceOptionDTO ChoiceOptionToDto(ChoiceOption choiceOption)
    {
        return new ChoiceOptionDTO()
        {
            Number = choiceOption.Number,
            Id = choiceOption.Id,
            Value = choiceOption.Value
        };
    }
}