using System.Security.Claims;
using AutoMapper;
using Microsoft.IdentityModel.JsonWebTokens;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.Services.AuthenticationServices;
using OnlineSurveyTool.Server.Services.AuthenticationServices.DTOs;
using OnlineSurveyTool.Server.Services.Commons;

namespace OnlineSurveyTool.Server.Services.Test.AuthenticationServices;

[TestFixture]
public class UserServiceTest
{
    private UserService _userService;
    private IUserRepo _userRepo;

    [SetUp]
    public void SetUp()
    {
        _userRepo = new UserRepoMock(new UserPopulator());
        var logger = new LoggerMock<UserService>();
                
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AutoMapperProfile>();
        });
        var mapper = config.CreateMapper();
        _userService = new UserService(_userRepo, mapper, logger);
    }
    
    [Test]
    public async Task ShouldSucceedForValidData()
    {
        var userDTO = new UserRegisterDTO
        {
            Login = "TestUser231",
            EMail = "test@mail.com",
            Password = "testPassword3!"
        };
        var userResult = await _userService.CreateUser(userDTO);
        Assert.That(userResult.IsSuccess);
        Assert.That(userResult.Value, Is.Not.Null);
    }

    [Test]
    public async Task ShouldSucceedForValidData2()
    {
        var userDTO = new UserRegisterDTO
        {
            Login = "Test_user_1234",
            EMail = "other@mail.pl",
            Password = "ThisIsAPassword123!"
        };
        var userResult = await _userService.CreateUser(userDTO);
        Assert.That(userResult.IsSuccess);
        Assert.That(userResult.Value, Is.Not.Null);
    }
    
    [Test]
    public async Task ShouldFailWithReasonForUserWithShortLogin()
    {
        await ShouldFailWithInvalidDataForValues("T", "t@mail.com", "testPassword4!");
    }

    [Test]
    public async Task ShouldFailWithReasonForUserWithInvalidEmail()
    {
        await ShouldFailWithInvalidDataForValues("ValidLogin", "mail.com", "testPassword4!");
    }

    [Test]
    public async Task ShouldFailWithReasonForUserWithShortPassword()
    {
        await ShouldFailWithInvalidDataForValues("ValidLogin", "valid@mail.com", "t!T3");
    }

    [Test]
    public async Task ShouldFailWithReasonForUserWithPasswordWithoutSmallLetter()
    {
        await ShouldFailWithInvalidDataForValues("ValidLogin", "valid@mail.com", "ALLCAPSPASSWORD!3");
    }

    [Test]
    public async Task ShouldFailWithReasonForUserWithPasswordWithoutLargeLetter()
    {
        await ShouldFailWithInvalidDataForValues("ValidLogin", "valid@mail.com", "allsmallpassword!3");
    }
    
    [Test]
    public async Task ShouldFailWithReasonForUserWithPasswordWithNoDigit()
    {
        await ShouldFailWithInvalidDataForValues("ValidLogin", "valid@mail.com", "NormalPassword!");
    }
    
    [Test]
    public async Task ShouldFailWithReasonForUserWithPasswordWithAllAlphanumeric()
    {
        await ShouldFailWithInvalidDataForValues("ValidLogin", "valid@mail.com", "NormalPassword123");
    }

    [Test]
    public async Task ShouldFailWithReasonForUserWithLoginContainingInvalidCharacters()
    {
        await ShouldFailWithInvalidDataForValues("Invalid user name", "valid@mail.com", "Password!1");
        await ShouldFailWithInvalidDataForValues("Invalid@Login", "valid@mail.com", "Password!1");
    }

    [Test]
    public async Task ShouldFailWithReasonForLoginContainingIllegalCharacters()
    {
        await ShouldFailWithInvalidDataForValues("Invalid!User@Name-()", "test@mail.com", "Password123!");
    }
    
    [Test]
    public async Task ShouldFailWithReasonForExistingUser()
    {
        var userDTO = new UserRegisterDTO
        {
            Login = "TestUser1",
            EMail = "test@mail.com",
            Password = "TestPassword1!"
        };

        var userResult = await _userService.CreateUser(userDTO);
        Assert.That(!userResult.IsSuccess);
        Assert.That(userResult.Reason, Is.EqualTo(UserCreationFailure.NameConflict));
    }

    [Test]
    public async Task ShouldReturnFalseIfLoginDoesNotExist()
    {
        var result = await _userService.CheckIfLoginExists("TestUser2137");
        Assert.That(!result);
    }
    
    [Test]
    public async Task ShouldReturnTrueIfLoginDoesExist()
    {
        var result = await _userService.CheckIfLoginExists("TestUser1");
        Assert.That(result);
    }

    [Test]
    public async Task ShouldReturnUserForValidClaimsPrincipal()
    {
        const string login = "TestUser1";
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, login)
        };
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        var result = await _userService.GetUserFromClaimsPrincipal(claimsPrincipal);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Login, Is.EqualTo(login));
    }

    [Test]
    public async Task ShouldReturnNullWhenLoginDoesNotExist()
    {
        const string login = "DoesNotExist!";
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, login)
        };
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));

        var user = await _userService.GetUserFromClaimsPrincipal(claimsPrincipal);
        Assert.That(user, Is.Null);
    }

    [Test] public async Task ShouldReturnNullWhenLoginIsNull()
    {
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        var user = await _userService.GetUserFromClaimsPrincipal(claimsPrincipal);
        Assert.That(user, Is.Null);
    }
    
    private async Task ShouldFailWithInvalidDataForValues(string login, string email, string password)
    {
        var userDTO = new UserRegisterDTO
        {
            Login = login,
            EMail = email,
            Password = password
        };

        var userResult = await _userService.CreateUser(userDTO);
        Assert.That(!userResult.IsSuccess);
        Assert.That(userResult.Reason, Is.EqualTo(UserCreationFailure.InvalidData));
    }
}