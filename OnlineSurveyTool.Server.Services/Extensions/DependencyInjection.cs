using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using OnlineSurveyTool.Server.Services.AuthenticationServices;
using OnlineSurveyTool.Server.Services.AuthenticationServices.Extensions;
using OnlineSurveyTool.Server.Services.AuthenticationServices.Interfaces;
using OnlineSurveyTool.Server.Services.SurveyServices;
using OnlineSurveyTool.Server.Services.SurveyServices.Extensions;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers.Interfaces;
using OnlineSurveyTool.Server.Services.SurveyServices.Interfaces;

namespace OnlineSurveyTool.Server.Services.Extensions
{
    public static class DependencyInjection
    {
        public static void AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    };

                    options.MapInboundClaims = false;
                });
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddAuthenticationServices();
            services.AddSurveyServices();
        }

        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
