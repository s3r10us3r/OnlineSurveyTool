using Microsoft.Extensions.DependencyInjection;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Utils.Extensions;

public static class DependencyInjection
{
    public static void AddUtils(this IServiceCollection services)
    {
        services.AddScoped<IGuidGenerator, GuidGenerator>();
        services.AddScoped<IChoiceOptionConverter, ChoiceOptionConverter>();
        services.AddScoped<IQuestionConverter, QuestionConverter>();
        services.AddScoped<IQuestionValidator, QuestionValidator>();
        services.AddScoped<ISurveyValidator, SurveyValidator>();
        services.AddScoped<ISurveyConverter, SurveyConverter>();
    }
}