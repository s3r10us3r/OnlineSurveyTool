using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyService.DTOs;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Utils;

public interface IChoiceOptionConverter
{ 
    ChoiceOption DtoToChoiceOption(ChoiceOptionDTO dto);
    ChoiceOptionDTO ChoiceOptionToDto(ChoiceOption choiceOption);
}