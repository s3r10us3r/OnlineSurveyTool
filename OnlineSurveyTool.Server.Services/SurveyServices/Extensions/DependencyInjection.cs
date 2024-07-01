using Microsoft.Extensions.DependencyInjection;
using OnlineSurveyTool.Server.Services.SurveyServices.Interfaces;
using OnlineSurveyTool.Server.Services.SurveyServices.Utils.Extensions;

namespace OnlineSurveyTool.Server.Services.SurveyServices.Extensions;

public static class DependencyInjection
{
    public static void AddSurveyServices(this IServiceCollection services)
    {
        services.AddUtils();
        services.AddScoped<ISurveyService, SurveyService>();
    }
}