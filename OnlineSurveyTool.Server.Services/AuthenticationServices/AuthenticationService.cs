using Microsoft.Extensions.Logging;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.DTOs;
using OnlineSurveyTool.Server.Services.AuthenticationServices.Interfaces;
using OnlineSurveyTool.Server.Services.Utils;
using OnlineSurveyTool.Server.Services.Utils.Interfaces;
using Crypt = BCrypt.Net.BCrypt;

namespace OnlineSurveyTool.Server.Services.AuthenticationServices
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        private readonly IUserRepo _userRepo;
        
        public AuthenticationService(IUserRepo userRepo, ILogger<AuthenticationService> logger) : base(logger)
        {
            _userRepo = userRepo;
        }

        public async Task<IResult<User>> AuthenticateUser(UserLoginDTO userDTO)
        {
            var user = await _userRepo.GetOne(userDTO.Login);
            if (user is null)
            {
                return Result<User>.Failure("User with this login does not exist!");
            }

            bool isVerified = Crypt.Verify(userDTO.Password, user.PasswordHash);
            if (!isVerified)
            {
                return Result<User>.Failure("Invalid password!");
            }

            return Result<User>.Success(user);
        }

    }
}
