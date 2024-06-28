using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyServices.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers.Interfaces;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Helpers;

public class SurveyConverter : ISurveyConverter
{
    private readonly IQuestionFactory _questionFactory;
    private readonly IQuestionMapper _questionMapper;

    public SurveyConverter(IQuestionFactory questionFactory, IQuestionMapper questionMapper)
    {
        _questionFactory = questionFactory;
        _questionMapper = questionMapper;
    }

    public SurveyDTO SurveyToDto(Survey survey) => new()
    {
        Id = survey.ExternalId,
        Name = survey.Name,
        OpeningDate = survey.OpeningDate,
        ClosingDate = survey.ClosingDate,
        IsOpen = survey.IsOpen,
        Questions = survey.Questions.Select(_questionFactory.MakeQuestionBase).ToList()
    };

    public Survey DtoToSurvey(SurveyDTO dto, User owner) => new()
    {
        ExternalId = dto.Id,
        IsOpen = dto.IsOpen,
        Name = dto.Name,
        OpeningDate = dto.OpeningDate,
        ClosingDate = dto.ClosingDate,
        Owner = owner,
        Questions = dto.Questions.Select(_questionMapper.MapDto)
    };
}