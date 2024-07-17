using Microsoft.AspNetCore.Mvc;
using OnlineSurveyTool.Server.Services.SurveyService.DTOs;

namespace OnlineSurveyTool.Server.Tests.Controllers.SurveyControllerTest;

[TestFixture]
public class SurveyControllerGetTest: SurveyControllerTestBase<string>
{
    protected override async Task<IActionResult> Action(string data) => await Controller.GetSurvey(data);

    [Test]
    public async Task ShouldReturnNotFoundForAClosedSurvey()
    {
        string data = "survey1";
        await ShouldReturnResultForData<NotFoundObjectResult>(data);
    }

    [Test]
    public async Task ShouldReturnNotFoundForANonExistentSurvey()
    {
        string data = "no";
        await ShouldReturnResultForData<NotFoundObjectResult>(data);
    }

    [Test]
    public async Task ShouldReturnSurveyForAValidSurvey()
    {
        string data = "survey2";
        await ShouldReturnResultForData<OkObjectResult>(data);
    }
}