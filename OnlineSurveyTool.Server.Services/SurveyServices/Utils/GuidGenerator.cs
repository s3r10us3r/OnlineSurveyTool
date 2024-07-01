namespace OnlineSurveyTool.Server.Services.SurveyServices.Utils;

public class GuidGenerator : IGuidGenerator
{
    public string GenerateGuid()
    {
        Guid guid = Guid.NewGuid();
        return guid.ToString();
    }
}