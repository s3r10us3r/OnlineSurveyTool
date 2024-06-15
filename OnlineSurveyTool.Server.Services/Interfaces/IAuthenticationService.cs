using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.Interfaces
{
    public interface IAuthenticationService : IService<User, IUserRepo>
    {
        IResult<User> CreateUser(string login, string eMail, string password);
        IResult<string> AuthenticateUser(string login, string password);
    }
}
