using OnlineSurveyTool.Server.Controllers;
using OnlineSurveyTool.Server.Services.SurveyServices;
using OnlineSurveyTool.Server.Services.SurveyServices.Utils;
using OnlineSurveyTool.Server.Services.Test.Mocks;

namespace OnlineSurveyTool.Server.Tests.Controllers.SurveyControllerTest;

public abstract class SurveyControllerTestBase<TD> : ControllerTestBase<SurveyController, TD>
{
    protected override SurveyController CreateController()
    {
        var uow = new UnitOfWorkMock();
        var config = ConfigurationCreator.MockConfig();
        var qV = new QuestionValidator(config);
        var sV = new SurveyValidator(qV);
        var esv = new EditSurveyValidator(qV, sV);
        var service = new SurveyService(uow, sV, esv, new LoggerMock<SurveyService>());
        var controller = new SurveyController(service, new LoggerMock<SurveyController>());
        return controller;
    }
}