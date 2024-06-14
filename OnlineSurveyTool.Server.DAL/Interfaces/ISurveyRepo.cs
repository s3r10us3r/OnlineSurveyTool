using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL.Interfaces
{
    public interface ISurveyRepo : IRepo<Survey>
    {
        Survey? GetOne(string token);

        IEnumerable<Survey> GetOpen(int userId);
    }
}
