using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OnlineSurveyTool.Server.DAL;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.DTOs;
using OnlineSurveyTool.Server.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Crypt = BCrypt.Net.BCrypt;

namespace OnlineSurveyTool.Server.Services
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        private readonly IUserRepo _userRepo;
        private readonly IJWTokenService _tokenService;

        public AuthenticationService(IUserRepo userRepo, ILogger<AuthenticationService> logger, IJWTokenService tokenService) : base(logger)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
        }

        public async Task<IResult<string>> AuthenticateUser(UserLoginDTO userDTO)
        {
            User? user = await _userRepo.GetOne(userDTO.Login);
            if (user is null)
            {
                return Result<string>.Failure("User with this login does not exist!");
            }

            bool isVerified = Crypt.Verify(userDTO.Password, user.PasswordHash);
            if (!isVerified)
            {
                return Result<string>.Failure("Invalid password!");
            }

            string tokenString = _tokenService.GenerateToken(user); 
            return Result<string>.Success(tokenString);
        }

    }
}
