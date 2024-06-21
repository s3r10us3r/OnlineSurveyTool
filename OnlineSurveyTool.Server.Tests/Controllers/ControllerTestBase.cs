using Microsoft.AspNetCore.Mvc;
namespace OnlineSurveyTool.Server.Tests.Controllers;

public abstract class ControllerTestBase<T, TD> where T: ControllerBase
{
    protected T Controller { get; }

    protected ControllerTestBase(T controller)
    {
        Controller = controller;
    }

    protected abstract Task<IActionResult> Action(TD data);

    protected async Task ShouldReturnResultForData<TR>(TD data) where TR: IActionResult
    {
        var result = await Action(data);
        Assert.That(result, Is.TypeOf<TR>());
    }
}