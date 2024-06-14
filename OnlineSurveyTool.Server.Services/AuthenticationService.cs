using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Crypt = BCrypt.Net.BCrypt;

namespace OnlineSurveyTool.Server.Services
{
    public class AuthenticationService : BaseService<User, IUserRepo>, IAuthenticationService
    {
        private readonly IConfiguration _config;

        public AuthenticationService(IUserRepo repo, ILogger<AuthenticationService> logger, IConfiguration config) : base(repo, logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public string? AuthenticateUser(User user, string password)
        {
            bool isVerified = Crypt.Verify(password, user.PasswordHash);
            if (!isVerified)
            {
                return null;
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

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public User? CreateUser(string login, string eMail, string password)
        {
            User user = new User
            {
                Login = login,
                EMail = eMail
            };

            if (Repo.GetOne(login) is not null)
            {
                throw new ArgumentException("User with this login already exist.");
            }

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

            return user;
        }

        private string HashPassword(string password)
        {
            string salt = Crypt.GenerateSalt();
            string hashedPassword = Crypt.HashPassword(password, salt);
            return hashedPassword;
        }
    }
}
