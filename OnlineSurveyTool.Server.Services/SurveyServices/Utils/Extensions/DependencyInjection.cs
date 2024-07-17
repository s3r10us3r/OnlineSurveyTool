using Microsoft.Extensions.DependencyInjection;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Utils.Extensions;

public static class DependencyInjection
{
    public static void AddUtils(this IServiceCollection services)
    {
        services.AddScoped<IQuestionValidator, QuestionValidator>();
        services.AddScoped<ISurveyValidator, SurveyValidator>();
    }
}