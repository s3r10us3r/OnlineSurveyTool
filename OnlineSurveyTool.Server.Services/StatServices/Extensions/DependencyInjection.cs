using Microsoft.Extensions.DependencyInjection;
using OnlineSurveyTool.Server.Services.StatServices.Interfaces;

namespace OnlineSurveyTool.Server.Services.StatServices.Extensions;

public static class DependencyInjection
{
    public static void AddStatServices(this IServiceCollection services)
    {
        services.AddScoped<IStatService, StatService>();
    }
}