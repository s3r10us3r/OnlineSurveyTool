using Microsoft.EntityFrameworkCore;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class UserRepo : BaseRepo<User>, IUserRepo
    {
        public UserRepo(OSTDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User?> GetOne(string Login)
        {
            return await Table.FirstOrDefaultAsync(u => u.Login == Login);
        }
    }
}
