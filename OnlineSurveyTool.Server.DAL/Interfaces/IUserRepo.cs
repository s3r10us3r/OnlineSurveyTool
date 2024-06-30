using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL.Interfaces
{
    public interface IUserRepo : IRepoNumericId<User>
    {
        Task<User?> GetOne(string login);
    }
}
