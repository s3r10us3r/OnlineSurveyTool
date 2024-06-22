using System.Security.Claims;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.AuthenticationServices.DTOs;
using OnlineSurveyTool.Server.Services.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.AuthenticationServices.Interfaces
{
    public interface IUserService
    {
        Task<IResult<UserDTO, UserCreationFailure>> CreateUser(UserRegisterDTO user);
        Task<User> GetUserFromClaimsPrincipal(ClaimsPrincipal claimsPrincipal);
        Task<bool> CheckIfLoginExists(string login);
    }
}
