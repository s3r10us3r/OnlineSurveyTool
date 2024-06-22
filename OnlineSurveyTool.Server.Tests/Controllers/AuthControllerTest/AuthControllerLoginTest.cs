using Microsoft.AspNetCore.Mvc;
using OnlineSurveyTool.Server.Services.AuthenticationServices.DTOs;

namespace OnlineSurveyTool.Server.Tests.Controllers.AuthControllerTest;

[TestFixture]
public class AuthControllerLoginTest: AuthControllerTestBase<UserLoginDTO>
{
    protected override async Task<IActionResult> Action(UserLoginDTO data) => await Controller.Login(data);

    [Test]
    public async Task ShouldReturnOkForValidUser()
    {
        await ShouldReturnResultForData<OkObjectResult>(new UserLoginDTO()
            { Login = "TestUser1", Password = "TestPassword1!" });
    }

    [Test]
    public async Task ShouldReturnUnauthorizedForInvalidData()
    {
        await ShouldReturnResultForData<UnauthorizedObjectResult>(new UserLoginDTO()
            { Login = "TestUser1", Password = "TestPassword2" });
        await ShouldReturnResultForData<UnauthorizedObjectResult>(new UserLoginDTO()
            { Login = "NotAuser", Password = "NotAPassword!2"});
    }
}
