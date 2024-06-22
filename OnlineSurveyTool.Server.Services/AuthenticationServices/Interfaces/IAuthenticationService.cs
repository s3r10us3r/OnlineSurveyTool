using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.AuthenticationServices.DTOs;
using OnlineSurveyTool.Server.Services.Utils.Interfaces;

namespace OnlineSurveyTool.Server.Services.AuthenticationServices.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IResult<User>> AuthenticateUser(UserLoginDTO loginDTO);
    }
}
