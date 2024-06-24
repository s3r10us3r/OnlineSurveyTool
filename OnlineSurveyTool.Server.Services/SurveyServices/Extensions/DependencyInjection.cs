using Microsoft.Extensions.DependencyInjection;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers.Interfaces;
using OnlineSurveyTool.Server.Services.SurveyServices.Interfaces;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Extensions;

public static class DependencyInjection
{
    public static void AddSurveyServices(this IServiceCollection services)
    {
        services.AddScoped<IQuestionFactory, QuestionFactory>();
        services.AddScoped<IQuestionMapper, QuestionMapper>();
        services.AddScoped<ISurveyConverter, SurveyConverter>();
        services.AddScoped<ISurveyValidator, SurveyValidator>();
        services.AddScoped<ISurveyService, SurveyService>();
    }
}