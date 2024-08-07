using OnlineSurveyTool.Server.Services.AuthenticationServices;
using OnlineSurveyTool.Server.Services.AuthenticationServices.DTOs;
using OnlineSurveyTool.Test.Utils.Mocks;
using OnlineSurveyTool.Test.Utils.Populators;

namespace OnlineSurveyTool.Server.Services.Test.AuthenticationServices;

[TestFixture]
public class AuthenticationServiceTest
{
    private AuthenticationService _authenticationService;

    public AuthenticationServiceTest() 
    {
        _authenticationService = new AuthenticationService(new UserRepoMock(new UserPopulator()), new LoggerMock<AuthenticationService>());
    }

    [Test]
    public async Task ShouldFailWhenInvalidPassword()
    {
        var dto = new UserLoginDTO
        {
            Login = "TestUser1",
            Password = "WrongPassword!"
        };
        
        var result = await _authenticationService.AuthenticateUser(dto);
        Assert.That(result.IsSuccess, Is.False);
    }

    [Test]
    public async Task ShouldFailWhenLoginDoesNotExist()
    {
        UserLoginDTO dto = new UserLoginDTO
        {
            Login = "NotAUser",
            Password = "WrongPassword!"
        };

        var result = await _authenticationService.AuthenticateUser(dto);
        Assert.That(result.IsSuccess, Is.False);
    }

    [Test]
    public async Task ShouldSucceedWithValidUser()
    {
        UserLoginDTO dto = new UserLoginDTO
        {
            Login = "TestUser1",
            Password = "TestPassword1!"
        };

        var result = await _authenticationService.AuthenticateUser(dto);
        Assert.That(result.IsSuccess, Is.True);
    }
}