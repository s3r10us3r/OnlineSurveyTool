using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;
using Crypt = BCrypt.Net.BCrypt;

namespace OnlineSurveyTool.Server.Services.Test.Mocks;

public class UserRepoMock: BaseMock<User>, IUserRepo
{
    public async Task<User?> GetOne(string Login)
    {
        return Entities.Find(u => u.Login == Login);
    }

    protected override void Populate()
    {
        Entities.Add(new User()
        {
            Id = 1,
            Login = "TestUser1",
            EMail = "test@mail.com",
            PasswordHash = HashPassword("TestPassword1!")
        });

        Entities.Add(new User()
        {
            Id = 2,
            Login = "TestUser2",
            EMail = "test@mail.com",
            PasswordHash = HashPassword("TestPassword2!")
        });
        
        Entities.Add(new User()
        {
            Id = 3,
            Login = "TestUser3",
            EMail = "test@mail.com",
            PasswordHash = HashPassword("TestPassword3!")
        });
        
        Entities.Add(new User()
        {
            Id = 4,
            Login = "TestUser4",
            EMail = "test@mail.com",
            PasswordHash = HashPassword("TestPassword4!")
        });
        
        Entities.Add(new User()
        {
            Id = 5,
            Login = "TestUser5",
            EMail = "test@mail.com",
            PasswordHash = HashPassword("TestPassword5!")
        });
    }

    private string HashPassword(string password)
    {
        string salt = Crypt.GenerateSalt();
        string hashedPassword = Crypt.HashPassword(password, salt);
        return hashedPassword;
    }
}