namespace OnlineSurveyTool.Server.Responses;

public class LoginResponse
{
    public string AccessToken { get; init; }
    public DateTime AccessTokenExpirationDateTime { get; init; }
    
}