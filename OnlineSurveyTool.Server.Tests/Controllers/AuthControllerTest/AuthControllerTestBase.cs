using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineSurveyTool.Server.Controllers;
using OnlineSurveyTool.Server.Services.AuthenticationServices;
using OnlineSurveyTool.Server.Services.Commons;
using OnlineSurveyTool.Test.Utils.Mocks;
using OnlineSurveyTool.Test.Utils.Populators;
using OnlineSurveyTool.Test.Utils.Stub;

namespace OnlineSurveyTool.Server.Tests.Controllers.AuthControllerTest;

public abstract class AuthControllerTestBase<TD> : ControllerTestBase<AuthController, TD>
{
    protected override AuthController CreateController()
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
        var cookieService = new CookieOptionsProvider(WebHostEnvironmentStubFactory.GetDevEnvironment());
        
        
        var controller = new AuthController(authenticationService, userService, jwTokenService,
            new LoggerMock<AuthController>(), cookieService);

        var context = new DefaultHttpContext();
        
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };
        return controller;
    }
}
