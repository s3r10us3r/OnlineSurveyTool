using System.Security.Claims;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.AuthenticationServices.Interfaces
{
    public interface IJWTokenService
    {
        string GenerateAccessToken(User user, out DateTime expiration);
        string GenerateRefreshToken(User user, out DateTime expiration);
        IResult<ClaimsPrincipal, RefreshFailure> GetClaimsPrincipalFromRefreshToken(string token);
    }
}
