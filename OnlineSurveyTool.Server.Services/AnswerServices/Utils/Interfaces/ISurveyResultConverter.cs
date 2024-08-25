using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.AnswerServices.DTOs;

namespace OnlineSurveyTool.Server.Services.AnswerServices.Utils.Interfaces;

public interface ISurveyResultConverter
{
   Task< SurveyResult> SurveyResultToModel(SurveyResultDTO dto);
}