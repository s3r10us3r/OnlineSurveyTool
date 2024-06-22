using Microsoft.AspNetCore.Mvc;
using OnlineSurveyTool.Server.Services.AuthenticationServices.DTOs;

namespace OnlineSurveyTool.Server.Tests.Controllers.AuthControllerTest;

[TestFixture]
public class AuthControllerRegisterTest : AuthControllerTestBase<UserRegisterDTO>
{
    protected override async Task<IActionResult> Action(UserRegisterDTO data) => await Controller.Register(data);
        
    [Test]
    public async Task ShouldReturnCreatedAtActionForAValidUser()
    {
        UserRegisterDTO data = new()
            { Login = "NewUser123", EMail = "new@mail.com", Password = "NewPassword!23" };
        await ShouldReturnResultForData<CreatedAtActionResult>(data);
    }

    [Test]
    public async Task ShouldReturnBadRequestForLoginTooShort()
    {
        UserRegisterDTO data = new() { Login = "l", EMail = "user@mail.com", Password = "ValidPassword!"};
        await ShouldReturnResultForData<BadRequestObjectResult>(data);
    }
    
    [Test]
    public async Task ShouldReturnBadRequestForLoginTooLong()
    {
        const string login = "SUPERLONGLOGINTHATISWAYTOOLONGTOBEALOGINANDITHAdSLIKETWOHUNDREDFIFTYSIXCHARACTERS";
        const string eMail = "user@mail.com";
        const string password = "ValidPassword#123";
        await ShouldReturnResultForData<BadRequestObjectResult>(
            new UserRegisterDTO {Login = login, EMail = eMail, Password = password});
    }

    [Test]
    public async Task ShouldReturnBadRequestForLoginWithIllegalSign()
    {
        await ShouldReturnResultForData<BadRequestObjectResult>(
            new UserRegisterDTO(){Login = "This!@#IsNotALogin", EMail = "this@mail.com", Password ="ThisIsAPassword!"});
        
    }

    [Test]
    public async Task ShouldReturnBadRequestForIllegalMail()
    {
        await ShouldReturnResultForData<BadRequestObjectResult>(
            new UserRegisterDTO() {Login = "This_is_a_login", EMail = "@mail.com", Password = "Password!3"});
        await ShouldReturnResultForData<BadRequestObjectResult>(
            new UserRegisterDTO(){Login = "This_is_a_login", EMail = "nota@mailcom", Password = "Password!3"});
        await ShouldReturnResultForData<BadRequestObjectResult>(
            new UserRegisterDTO(){Login = "This_is_a_login", EMail = "nota@mail.", Password = "Password!3"});
    }

    [Test]
    public async Task ShouldReturnBadRequestForTooShortPassword()
    {
        await ShouldReturnResultForData<BadRequestObjectResult>(
            new UserRegisterDTO(){Login = "NewUser123", EMail = "new@mail.com", Password = "a#3A"});
    }

    [Test]
    public async Task ShouldReturnBadRequestForTooLongPassword()
    {
        await ShouldReturnResultForData<BadRequestObjectResult>(
            new UserRegisterDTO(){Login = "NewUser123", EMail = "new@mail.com",
            Password = "DSAJDOAISJDOIASJDOSAJI#1233212IOJDOIJSADIOsdadsadsaad1231!!!!!dasdasokdosajkidsaokdsaoidkaodsads"});
    }

    [Test]
    public async Task ShouldReturnBadRequestForIllegalPasswords()
    {
        await ShouldReturnResultForData<BadRequestObjectResult>(
            new UserRegisterDTO() {Login = "NewUser123", EMail = "new@mail.com", Password = "123KPP KKKKaaaa!!"});
        await ShouldReturnResultForData<BadRequestObjectResult>( 
            new UserRegisterDTO() {Login = "NewUser123", EMail = "new@mail.com", Password = "AllAlphaNumeric123"});
        await ShouldReturnResultForData<BadRequestObjectResult>( 
            new UserRegisterDTO() {Login = "NewUser123", EMail = "new@mail.com", Password = "NoDigitIn!"});
        await ShouldReturnResultForData<BadRequestObjectResult>(
            new UserRegisterDTO() {Login = "NewUser123", EMail = "new@mail.com", Password = "allsmall123!"});
        await ShouldReturnResultForData<BadRequestObjectResult>(
            new UserRegisterDTO() {Login = "NewUser123", EMail = "new@mail.com", Password = "ALLLARGE123!"});
    }

    [Test]
    public async Task ShouldReturnConflictForExistingUsername()
    {
        await ShouldReturnResultForData<ConflictObjectResult>(
            new UserRegisterDTO() {Login = "TestUser1", EMail = "test@mail.com", Password = "TestPassword1!"});
    }
}
