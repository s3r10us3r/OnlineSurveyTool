using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL
{
    public class UserRepo : BaseRepo<User>, IUserRepo
    {
        public UserRepo(OSTDbContext dbContext) : base(dbContext)
        {
        }

        public User? GetOne(string Login)
        {
            return Table.FirstOrDefault(u => u.Login == Login);
        }
    }
}
