using AutoMapper;
using OnlineSurveyTool.Server.Controllers;
using OnlineSurveyTool.Server.Services.AuthenticationServices;
using OnlineSurveyTool.Server.Services.Commons;
using OnlineSurveyTool.Server.Services.Test.Mocks;

namespace OnlineSurveyTool.Server.Tests.Controllers.AuthControllerTest;

public abstract class AuthControllerTestBase<TD> : ControllerTestBase<AuthController, TD>
{
    private static AuthController CreateController()
    {
        var config = new MapperConfiguration(cfg =>
        { 
            cfg.AddProfile<AutoMapperProfile>();
        });
        var mapper = config.CreateMapper();
        var userRepo = new UserRepoMock(new UserPopulator());
        var userService = new UserService(userRepo, mapper, new LoggerMock<UserService>());
        var jwTokenService = new JWTokenService(new LoggerMock<JWTokenService>(), ConfigurationCreator.MockConfig());
        var authenticationService = new AuthenticationService(userRepo, new LoggerMock<AuthenticationService>());

        var controller = new AuthController(authenticationService, userService, jwTokenService,
            new LoggerMock<AuthController>());
        return controller;
    }

    protected AuthControllerTestBase() : base(CreateController())
    {
    }
}
