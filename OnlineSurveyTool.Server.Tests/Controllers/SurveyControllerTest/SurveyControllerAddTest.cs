using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineSurveyTool.Server.Services.SurveyService.DTOs;
using ClaimsIdentity = System.Security.Claims.ClaimsIdentity;

namespace OnlineSurveyTool.Server.Tests.Controllers.SurveyControllerTest;

[TestFixture]
public class SurveyControllerAddTest : SurveyControllerTestBase<SurveyDTO>
{
    protected override async Task<IActionResult> Action(SurveyDTO data) => await Controller.Add(data);


    private SurveyDTO _testDto;

    [SetUp]
    public void SetUp()
    {
        _testDto = new SurveyDTO()
        {
            Id = "survey3",
            Name = "Survey 3",
            Questions =
            [
                new QuestionDTO()
                {
                    Id = "s3q1", Number = 0, Value = "This is a question", Type = "Textual", CanBeSkipped = false,
                    Minimum = 1, Maximum = 100
                }
            ]
        };
        
        SetLoginInContext("TestUser1");
    }

    [Test]
    public async Task ShouldReturnCreatedAtActionForAValidSurvey()
    {
        await ShouldReturnResultForData<CreatedAtActionResult>(_testDto);
    }

    [Test]
    public async Task ShouldReturnBadRequestForAnInvalidSurvey()
    {
        _testDto.Questions[0].Number = -1;
        await ShouldReturnResultForData<BadRequestObjectResult>(_testDto);
    }
}