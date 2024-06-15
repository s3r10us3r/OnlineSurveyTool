using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL.Interfaces
{
    public interface ISurveyRepo : IRepo<Survey>
    {
        Task<Survey?> GetOne(string token);

        Task<List<Survey>> GetOpen(int userId);
    }
}
