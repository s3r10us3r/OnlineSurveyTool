using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class SurveyRepo : BaseRepo<Survey>, ISurveyRepo
    {
        public SurveyRepo(OSTDbContext dbContext) : base(dbContext)
        {
        }

        public new Survey? GetOne(int id)
        {
            var survey = Table.Find(id);
            if (survey is not null && !survey.IsArchived)
            {
                return survey;
            }
            return null;
        }

        public Survey? GetOne(string token)
        {
            var survey = Table.FirstOrDefault(s => s.Token == token);
            if (survey is not null && !survey.IsArchived)
            {
                return survey;
            }
            return null;

        }

        public new List<Survey> GetAll()
        {
            return Table.Where(s => !s.IsArchived).ToList();
        }

        public IEnumerable<Survey> GetOpen(int ownerId)
        {
            return Table.Where(s => s.OwnerId == ownerId && !s.IsArchived && s.OwnerId == ownerId);
        }
    }
}
