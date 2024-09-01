using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL.Interfaces
{
    public interface ISurveyRepo : IRepoStringId<Survey>
    {
        public Task LoadResults(Survey survey);
    }
}
