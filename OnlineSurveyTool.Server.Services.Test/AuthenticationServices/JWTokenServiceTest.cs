using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.AuthenticationServices;
using OnlineSurveyTool.Server.Services.Test.Mocks;

namespace OnlineSurveyTool.Server.Services.Test.AuthenticationServices;

[TestFixture]
public class JWTokenServiceTest
{
    private readonly JWTokenService _service;
    private readonly IUserRepo _userRepo;
    private IConfiguration _config;
    
    public JWTokenServiceTest()
    {
        _config = ConfigurationCreator.MockConfig();
        _service = new JWTokenService(new LoggerMock<JWTokenService>(), _config);
        _userRepo = new UserRepoMock(new UserPopulator());
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); 
    }

    [Test]
    public async Task ShouldGenerateAccessTokenForAValidUser()
    {
        var user = await _userRepo.GetOne(1);
        var token = _service.GenerateAccessToken(user, out var expiration);
        ValidateToken(token, user, "access");
    }

    [Test]
    public async Task ShouldGenerateRefreshTokenForAValidUser()
    {
        var user = await _userRepo.GetOne(1);
        var token = _service.GenerateRefreshToken(user, out var expiration);
        ValidateToken(token, user, "refresh");
    }

    [Test]
    public async Task ShouldReturnSuccessWithValidClaimsForValidRefreshToken()
    {
        var user = await _userRepo.GetOne(1);
        var token = _service.GenerateRefreshToken(user, out var expiration);
        var result = _service.GetClaimsPrincipalFromRefreshToken(token);
        
        Assert.That(result.IsSuccess, Is.True);

        var principal = result.Value;
        Assert.That(principal, Is.Not.Null);
        
        var subClaim = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
        Assert.That(subClaim, Is.Not.Null);
        Assert.That(subClaim.Value, Is.EqualTo(user.Login));

        var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);
        Assert.That(emailClaim, Is.Not.Null);
        Assert.That(emailClaim.Value, Is.EqualTo(user.EMail));

        var typeClaim = principal.Claims.FirstOrDefault(c => c.Type == "type");
        Assert.That(typeClaim, Is.Not.Null);
        Assert.That(typeClaim.Value, Is.EqualTo("refresh"));
    }

    [Test]
    public async Task ShouldReturnFailureWithCorrectReasonForExpiredRefreshToken()
    {
        var user = await _userRepo.GetOne(1);
        var token = GenerateExpiredToken(user);
        var result = _service.GetClaimsPrincipalFromRefreshToken(token);
        
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Reason, Is.EqualTo(RefreshFailure.SecurityTokenExpired));
    }

    [Test]
    public async Task ShouldReturnFailureWithCorrectReasonForInvalidToken()
    {
        var user = await _userRepo.GetOne(1);
        var token = GenerateInvalidToken(user);
        var result = _service.GetClaimsPrincipalFromRefreshToken(token);
        
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Reason, Is.EqualTo(RefreshFailure.SecurityTokenInvalid));
    }

    [Test]
    public async Task ShouldReturnFailureWithCorrectReasonForTokenWithWrongKey()
    {
         var user = await _userRepo.GetOne(1);
        var token = GenerateTokenWithWrongKey(user);
        var result = _service.GetClaimsPrincipalFromRefreshToken(token);
        
        Assert.That(result.IsSuccess, Is.False);
        Assert.That(result.Reason, Is.EqualTo(RefreshFailure.SecurityTokenInvalid));
    }
    
    private void ValidateToken(string token, User user, string type)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

        var validationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _config["Jwt:Issuer"],
            ValidAudience = _config["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        SecurityToken validatedToken;
        var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
        
        Assert.That(principal, Is.Not.Null);
        Assert.That(validatedToken, Is.Not.Null);

        var subClaim = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
        Assert.That(subClaim, Is.Not.Null);
        Assert.That(subClaim.Value, Is.EqualTo(user.Login));

        var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);
        Assert.That(emailClaim, Is.Not.Null);
        Assert.That(emailClaim.Value, Is.EqualTo(user.EMail));

        var typeClaim = principal.Claims.FirstOrDefault(c => c.Type == "type");
        Assert.That(typeClaim, Is.Not.Null);
        Assert.That(typeClaim.Value, Is.EqualTo(type));
    }

    private string GenerateExpiredToken(User user)
    {
        return GenerateCustomToken(user, expiryTime: -5);
    }
    
    private string GenerateInvalidToken(User user)
    {
        return GenerateCustomToken(user, issuer: "invalidIssuer", audience: "invalidAudience");
    }

    private string GenerateTokenWithWrongKey(User user)
    {
        return GenerateCustomToken(user, keyString: "THIS_IS_A_WRONG_VERY_WRONG_KEY_THAT_HAS_TO_BE_LONG");
    }
    
    private string GenerateCustomToken(User user, string issuer = "", string audience = "", string keyString = "", int expiryTime = 30)
    {
        issuer = issuer == "" ? _config["Jwt:Issuer"]! : issuer;
        audience = audience == "" ? _config["Jwt:Audience"]! : audience;
        keyString = keyString == "" ? _config["Jwt:Key"]! : keyString;
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Login),
            new Claim(JwtRegisteredClaimNames.Email, user.EMail),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("type", "refresh")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(expiryTime),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}