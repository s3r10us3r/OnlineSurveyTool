using Microsoft.Extensions.DependencyInjection;
using OnlineSurveyTool.Server.Services.SurveyServices.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Utils.Extensions;

public static class DependencyInjection
{
    public static void AddUtils(this IServiceCollection services)
    {
        services.AddScoped<IQuestionValidator, QuestionValidator>();
        services.AddScoped<ISurveyValidator, SurveyValidator>();
        services.AddScoped<IEditSurveyValidator, EditSurveyValidator>();
    }
}