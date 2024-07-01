using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyService.DTOs;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Utils;

public class SurveyConverter : ISurveyConverter
{
    private IQuestionConverter _questionConverter;
    private IGuidGenerator _guidGenerator;
    
    public SurveyConverter(IQuestionConverter questionConverter, IGuidGenerator guidGenerator)
    {
        _questionConverter = questionConverter;
        _guidGenerator = guidGenerator;
    }
    
    public Survey DtoToSurvey(SurveyDTO surveyDto)
    { 
        return new Survey()
        {
            Id = surveyDto.Id ?? _guidGenerator.GenerateGuid(),
            Name = surveyDto.Name,
            Questions = surveyDto.Questions.Select(_questionConverter.DtoToQuestion)
        };
    }

    public SurveyDTO SurveyToDto(Survey survey)
    {
        return new SurveyDTO()
        {
            Id = survey.Id,
            Name = survey.Name,
            Questions = survey.Questions.Select(_questionConverter.QuestionToDto).ToList()
        };
    }
}