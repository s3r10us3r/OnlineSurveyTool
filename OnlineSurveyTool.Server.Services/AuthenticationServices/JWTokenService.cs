using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.AuthenticationServices.Interfaces;
using OnlineSurveyTool.Server.Services.Utils;
using OnlineSurveyTool.Server.Services.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.AuthenticationServices
{
    public class JWTokenService : BaseService, IJWTokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _signingKey;
        
        public JWTokenService(ILogger<BaseService> logger, IConfiguration config) : base(logger)
        {
            _config = config;
            _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        }

        public string GenerateAccessToken(User user, out DateTime expiration)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Login),
                new Claim(JwtRegisteredClaimNames.Email, user.EMail),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("type", "access")
            };

            string expiryString = _config["Jwt:AccessTokenExpiryMinutes"]!;
            int expiryMinutes = int.Parse(expiryString);
            expiration = DateTime.UtcNow.AddMinutes(expiryMinutes);
            string token = GenerateToken(claims, expiration);
            Logger.LogInformation(string.Format("New access token: {0} generated for user {1} at {2}", user.Login, token,
                DateTime.Now.ToString(CultureInfo.InvariantCulture)));
            return token;
        }

        public string GenerateRefreshToken(User user, out DateTime expiration)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Login),
                new Claim(JwtRegisteredClaimNames.Email, user.EMail),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("type", "refresh")
            };

            string expiryString = _config["Jwt:RefreshTokenExpiryMinutes"]!;
            int expiryMinutes = int.Parse(expiryString);
            expiration = DateTime.UtcNow.AddMinutes(expiryMinutes);
            string token = GenerateToken(claims, expiration);
            Logger.LogInformation(string.Format("New refresh token: {0} generated for user {1} at {2}", user.Login, token,
                DateTime.Now.ToString(CultureInfo.InvariantCulture)));
            return token;
        }

        public IResult<ClaimsPrincipal, RefreshFailure> GetClaimsPrincipalFromRefreshToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,
                ValidateLifetime = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                return Result<ClaimsPrincipal, RefreshFailure>.Success(principal);
            }
            catch (SecurityTokenExpiredException e)
            {
                Logger.LogInformation("Expired security token sent token: {token}", token);
                return Result<ClaimsPrincipal, RefreshFailure>.Failure("Security token expired",
                    RefreshFailure.SecurityTokenExpired);
            }
            catch (SecurityTokenException e)
            {
                return Result<ClaimsPrincipal, RefreshFailure>.Failure(e.Message, RefreshFailure.SecurityTokenInvalid);
            }
        }

        private string GenerateToken(IEnumerable<Claim> claims, DateTime expiration)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiration,
                SigningCredentials = new SigningCredentials(_signingKey,  SecurityAlgorithms.HmacSha256Signature),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public enum RefreshFailure
    {
        SecurityTokenExpired, SecurityTokenInvalid
    }
}
