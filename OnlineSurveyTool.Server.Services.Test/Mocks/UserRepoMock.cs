using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using Crypt = BCrypt.Net.BCrypt;

namespace OnlineSurveyTool.Server.Services.Test.Mocks;

public class UserRepoMock: BaseMock<User, int>, IUserRepo
{
    public UserRepoMock(IPopulator<User, int> populator) : base(populator)
    {
    }

    public async Task<User?> GetOne(string login)
    {
        return Entities.Find(u => u.Login == login);
    }
}