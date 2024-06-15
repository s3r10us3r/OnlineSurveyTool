using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.DTOs;

namespace OnlineSurveyTool.Server.Services.Interfaces
{
    public interface IUserService
    {
        Task<IResult<UserDTO>> CreateUser(UserRegisterDTO user);
    }
}
