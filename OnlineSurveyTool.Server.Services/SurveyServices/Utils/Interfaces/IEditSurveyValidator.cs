using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyService.DTOs;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Utils.Interfaces;

public interface IEditSurveyValidator
{
    bool ValidateSurveyEdit(SurveyEditDto edit, Survey survey, out string message);
}