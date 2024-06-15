using OnlineSurveyTool.Server.DTOs;

namespace OnlineSurveyTool.Server.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IResult<string>> AuthenticateUser(UserLoginDTO loginDTO);
    }
}
