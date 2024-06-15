using OnlineSurveyTool.Server.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSurveyTool.Server.DAL.Interfaces
{
    public interface IUserRepo : IRepo<User>
    {
        Task<User?> GetOne(string Login);
    }
}
