using System.ComponentModel.DataAnnotations;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers.Interfaces;

namespace OnlineSurveyTool.Server.Services.SurveyServices.DTOs;

public class SurveyDTO
{
    [Required]
    [StringLength(256, MinimumLength = 256)]
    public string Token { get; set; }
    
    [Required(AllowEmptyStrings = false)]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; }
    
    public DateTime? OpeningDate { get; set; }

    public DateTime? ClosingDate { get; set; }

    [Required]
    public bool IsOpen { get; set; }
    
    [Required]
    public IEnumerable<QuestionBase> Questions { get; set; }

    public SurveyDTO()
    {
    }

    public SurveyDTO(Survey survey, IQuestionFactory factory)
    {
        Token = survey.Token;
        Name = survey.Name;
        OpeningDate = survey.OpeningDate;
        ClosingDate = survey.ClosingDate;
        IsOpen = survey.IsOpen;
        Questions = survey.Questions.Select(factory.MakeQuestion);
    }

    public Survey ToSurvey(User owner, string surveyToken)
    {
        var survey = new Survey
        {
            Token = surveyToken,
            Owner = owner,
            Name = Name,
            OpeningDate = OpeningDate,
            ClosingDate = ClosingDate,
            IsOpen = IsOpen,
            Questions = Questions.Select(q => q.ToQuestion())
        };
        return survey;
    }
}