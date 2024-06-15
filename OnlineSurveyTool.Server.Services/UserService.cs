using AutoMapper;
using Microsoft.Extensions.Logging;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.DTOs;
using OnlineSurveyTool.Server.Services.Interfaces;
using System.Text.RegularExpressions;
using Crypt = BCrypt.Net.BCrypt;

namespace OnlineSurveyTool.Server.Services
{
    public class UserService : BaseService, IUserService
    {
        private const string EMAIL_REGEX = @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])";
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public UserService(IUserRepo userRepo, IMapper mapper, ILogger<BaseService> logger) : base(logger)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<IResult<UserDTO>> CreateUser(UserRegisterDTO userDTO)
        {
            var validationResult = await ValidateUserRegisterDTO(userDTO);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            User user = _mapper.Map<User>(userDTO);

            string passwordHash = HashPassword(userDTO.Password);
            user.PasswordHash = passwordHash;
            int result;
            try
            {
                result = await _userRepo.Add(user);
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Error occured while adding user to db.");
                throw new Exception("Error adding user to the database.", e);
            }
            if (result == 0)
            {
                Logger.LogError("User could not be added to the database.");
                throw new Exception("User could not be added to the database.");
            }

            return Result<UserDTO>.Success(new UserDTO { Id = user.Id, Login = user.Login, Email = user.EMail });
        }

        private async Task<Result<UserDTO>> ValidateUserRegisterDTO(UserRegisterDTO userRegisterDTO)
        {

            if (!ValidateLogin(userRegisterDTO.Login, out string loginMessage))
            {
                return Result<UserDTO>.Failure(loginMessage);
            }
            if (!ValidateEMail(userRegisterDTO.EMail, out string emailMessage))
            {
                return Result<UserDTO>.Failure(emailMessage);
            }
            if (!ValidatePassword(userRegisterDTO.Password, out string passwordMessage))
            {
                return Result<UserDTO>.Failure(passwordMessage);
            }
            if (await _userRepo.GetOne(userRegisterDTO.Login) is not null)
            {
                return Result<UserDTO>.Failure("User with this login already exists!");
            }

            return Result<UserDTO>.Success(null);
        }

        private string HashPassword(string password)
        {
            string salt = Crypt.GenerateSalt();
            string hashedPassword = Crypt.HashPassword(password, salt);
            return hashedPassword;
        }

        private bool ValidatePassword(string password, out string message)
        {
            message = "";

            bool hasNonAlphanumeric = Regex.IsMatch(password, @"[^a-zA-Z0-9]");
            bool hasUpperCase = Regex.IsMatch(password, @"[A-Z]");
            bool hasLowerCase = Regex.IsMatch(password, @"[a-z]");
            bool hasDigit = Regex.IsMatch(password, @"\d");

            if (password.Length < 8)
            {
                message = "Password's length must be greater than 8!";
            }
            if (!hasNonAlphanumeric)
            {
                message = "Password must contain at least one non alphanumeric character!";
            }
            if (!hasUpperCase)
            {
                message = "Password must contain at least one uppercase letter!";
            }
            if (!hasLowerCase)
            {
                message = "Password must contain at least one lowercase letter!";
            }
            if (!hasDigit)
            {
                message = "Password must contain at least one digit!";
            }

            return password.Length > 8 && hasNonAlphanumeric && hasUpperCase && hasLowerCase && hasDigit;
        }

        private bool ValidateLogin(string login, out string message)
        {
            bool hasValidLength = login.Length >= 8;
            if (!hasValidLength)
            {
                message = "Login is too short!";
                return false;
            }
            message = "";
            return true;
        }

        private bool ValidateEMail(string email, out string message)
        {
            bool satisfiesEmailRegex = Regex.IsMatch(email, EMAIL_REGEX);
            if (!satisfiesEmailRegex)
            {
                message = "Invalid eMail";
                return false;
            }
            message = "";
            return true;
        }
    }
}
