using Microsoft.AspNetCore.Http;

namespace OnlineSurveyTool.Server.Services.AuthenticationServices.Interfaces;

public interface ICookieOptionsProvider
{
    CookieOptions GetCookieOptions();
}