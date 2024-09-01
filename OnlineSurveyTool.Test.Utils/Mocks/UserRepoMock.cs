using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Test.Utils.Populators;

namespace OnlineSurveyTool.Test.Utils.Mocks;

public class UserRepoMock: BaseMock<User, int>, IUserRepo
{
    public UserRepoMock(IPopulator<User, int> populator) : base(populator)
    {
    }

    public async Task<User?> GetOne(string login)
    {
        return Entities.Find(u => u.Login == login);
    }

    public async Task LoadSurveys(User user)
    {
        throw new NotImplementedException();
    }
}