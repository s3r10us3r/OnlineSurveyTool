using Microsoft.Extensions.DependencyInjection;
using OnlineSurveyTool.Server.Services.AuthenticationServices.Interfaces;

namespace OnlineSurveyTool.Server.Services.AuthenticationServices.Extensions;

public static class DependencyInjection
{
    public static void AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IJWTokenService, JWTokenService>();
    }
}