using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.Interfaces
{
    public interface IAuthenticationService : IService<User, IUserRepo>
    {
        User? CreateUser(string login, string eMail, string password);
        string AuthenticateUser(User user, string password);
    }
}
