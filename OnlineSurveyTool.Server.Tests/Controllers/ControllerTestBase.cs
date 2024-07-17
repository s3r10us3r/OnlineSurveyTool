using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace OnlineSurveyTool.Server.Tests.Controllers;

//TD is a dto object that should be returned by tested methods
//First you make a class that corresponds to the entire controller then 
//a class that corresponds to a tested method 
public abstract class ControllerTestBase<T, TD> where T: ControllerBase
{
    protected T Controller { get; }

    protected ControllerTestBase()
    {
        Controller = CreateController();
    }

    protected abstract T CreateController();
    
    protected abstract Task<IActionResult> Action(TD data);

    protected async Task ShouldReturnResultForData<TR>(TD data) where TR: IActionResult
    {
        var result = await Action(data);
        Assert.That(result, Is.TypeOf<TR>());
    }

    protected async Task ShouldReturnResultForData<TR>(TD data, Action<TR> additionalAssertions)
        where TR : IActionResult
    {
        var result = await Action(data);
        Assert.That(result, Is.TypeOf<TR>());
        additionalAssertions((TR)result);
    }

    protected void SetLoginInContext(string login)
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.Name, login)
        }, "mock"));
        
        Controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext() {User = user}
        };
    }
}