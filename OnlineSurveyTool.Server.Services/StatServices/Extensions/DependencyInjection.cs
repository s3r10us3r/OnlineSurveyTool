using Microsoft.Extensions.DependencyInjection;
using OnlineSurveyTool.Server.Services.StatServices.Interfaces;
using OnlineSurveyTool.Server.Services.StatServices.Utils.Extensions;

namespace OnlineSurveyTool.Server.Services.StatServices.Extensions;

public static class DependencyInjection
{
    public static void AddStatServices(this IServiceCollection services)
    {
        services.AddStatUtils();
        services.AddScoped<IStatService, StatService>();
    }
}