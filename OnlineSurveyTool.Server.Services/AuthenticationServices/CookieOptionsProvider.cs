using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using OnlineSurveyTool.Server.Services.AuthenticationServices.Interfaces;

namespace OnlineSurveyTool.Server.Services.AuthenticationServices;

public class CookieOptionsProvider : ICookieOptionsProvider
{
    private readonly bool _isDev;
    
    public CookieOptionsProvider(IWebHostEnvironment env)
    {
        _isDev = env.IsDevelopment();
    }
    
    public CookieOptions GetCookieOptions()
    {
        if (_isDev)
        {
            return GetDevCookieOptions();
        }

        return GetProdCookieOptions();
    }

    private static CookieOptions GetDevCookieOptions()
    {
        return new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(7)
        };
    }

    private static CookieOptions GetProdCookieOptions()
    {
        throw new NotImplementedException();
    }
}