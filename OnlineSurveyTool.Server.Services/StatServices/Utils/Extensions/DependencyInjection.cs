using Microsoft.Extensions.DependencyInjection;
using OnlineSurveyTool.Server.Services.StatServices.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.StatServices.Utils.Extensions;

public static class DependencyInjection
{
    public static void AddStatUtils(this IServiceCollection services)
    {
        services.AddScoped<INumAnalyzer, NumAnalyzer>();
        services.AddScoped<ITextAnalyzer, TextAnalyzer>();
        services.AddScoped<IAnswerStatsHelper, AnswerStatsHelper>();
    }
}