using System.Security.Claims;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.AuthenticationServices.Interfaces
{
    public interface IJWTokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken(User user);
        IResult<ClaimsPrincipal, RefreshFailure> GetClaimsPrincipalFromRefreshToken(string token);
    }
}
