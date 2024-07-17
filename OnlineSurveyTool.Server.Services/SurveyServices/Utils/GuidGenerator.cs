namespace OnlineSurveyTool.Server.Services.SurveyServices.Utils;

public static class GuidGenerator
{
    public static string GenerateGuid()
    {
        Guid guid = Guid.NewGuid();
        return guid.ToString();
    }
}