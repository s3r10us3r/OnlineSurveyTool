using Microsoft.Extensions.DependencyInjection;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.AnswerServices.Interfaces;
using OnlineSurveyTool.Server.Services.AnswerServices.Utils;
using OnlineSurveyTool.Server.Services.AnswerServices.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.AnswerServices.Extensions;

public static class DependencyInjection
{
    public static void AddAnswerServices(this IServiceCollection services)
    {
        services.AddScoped<ISurveyResultConverter, SurveyResultConverter>();
        services.AddScoped<ISurveyResultValidator, SurveyResultValidator>();
        services.AddScoped<IAnswerService, AnswerService>();
    }
}