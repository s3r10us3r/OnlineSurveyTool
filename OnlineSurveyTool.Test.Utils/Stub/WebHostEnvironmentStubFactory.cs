using Microsoft.AspNetCore.Hosting;

namespace OnlineSurveyTool.Test.Utils.Stub;

public static class WebHostEnvironmentStubFactory
{
    public static IWebHostEnvironment GetDevEnvironment()
    {
        return new WebHostEnvironmentStub()
        {
            EnvironmentName = "Development"
        };
    }

    public static IWebHostEnvironment GetProdEnvironment()
    {
        return new WebHostEnvironmentStub()
        {
            EnvironmentName = "Production"
        };
    }
}