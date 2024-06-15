using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Crypt = BCrypt.Net.BCrypt;

namespace OnlineSurveyTool.Server.Services
{
    public class AuthenticationService : BaseService<User, IUserRepo>, IAuthenticationService
    {
        private readonly IConfiguration _config;
        private const string EMAIL_REGEX = @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])";

        public AuthenticationService(IUserRepo repo, ILogger<AuthenticationService> logger, IConfiguration config) : base(repo, logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public IResult<string> AuthenticateUser(string login, string password)
        {
            User? user = Repo.GetOne(login);
            if (user is null)
            {
                return Result<string>.Failure("User with this login does not exist!");
            }

            bool isVerified = Crypt.Verify(password, user.PasswordHash);
            if (!isVerified)
            {
                return Result<string>.Failure("Invalid password!");
            }

            string securityKeyString = _config["Jwt:Key"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKeyString));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiryMinutes"]));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Login),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: credentials
                );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Result<string>.Success(tokenString);
        }

        public IResult<User> CreateUser(string login, string eMail, string password)
        {
            if (!ValidateLogin(login, out string loginMessage))
            {
                return Result<User>.Failure(loginMessage);
            }
            if (!ValidateEMail(eMail, out string emailMessage))
            {
                return Result<User>.Failure(emailMessage);
            }
            if (!ValidatePassword(password, out string passwordMessage))
            {
                return Result<User>.Failure(passwordMessage);
            }
            if (Repo.GetOne(login) is not null)
            {
                return Result<User>.Failure("User with this login already exists!");
            }

            User user = new User
            {
                Login = login,
                EMail = eMail
            };

            string passwordHash = HashPassword(password);
            user.PasswordHash = passwordHash;
            int result;
            try
            {
                result = Repo.Add(user);
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

            return Result<User>.Success(user);
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
