using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL.Interfaces
{
    public interface IUserRepo : IRepo<User>
    {
        Task<User?> GetOne(string Login);
    }
}
