using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.Test.Mocks;

public class UserPopulator : IPopulator<User, int>
{
    public List<User> Populate()
    {
        List<User> entities =
        [
            new User()
            {
                Id = 1,
                Login = "TestUser1",
                EMail = "test@mail.com",
                PasswordHash = HashPassword("TestPassword1!")
            },

            new User()
            {
                Id = 2,
                Login = "TestUser2",
                EMail = "test@mail.com",
                PasswordHash = HashPassword("TestPassword2!")
            },


            new User()
            {
                Id = 3,
                Login = "TestUser3",
                EMail = "test@mail.com",
                PasswordHash = HashPassword("TestPassword3!")
            },


            new User()
            {
                Id = 4,
                Login = "TestUser4",
                EMail = "test@mail.com",
                PasswordHash = HashPassword("TestPassword4!")
            },


            new User()
            {
                Id = 5,
                Login = "TestUser5",
                EMail = "test@mail.com",
                PasswordHash = HashPassword("TestPassword5!")
            }

        ];

        return entities;
    }

    private string HashPassword(string password)
    {
        string salt = BCrypt.Net.BCrypt.GenerateSalt();
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
        return hashedPassword;
    }
}