using Microsoft.EntityFrameworkCore;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class UserRepo : BaseRepoNumericId<User>, IUserRepo
    {
        public UserRepo(OstDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User?> GetOne(string login)
        {
            return await Table.FirstOrDefaultAsync(u => u.Login == login);
        }
    }
}
