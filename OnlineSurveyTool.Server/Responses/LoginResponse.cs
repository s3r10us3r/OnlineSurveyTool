namespace OnlineSurveyTool.Server.Responses;

public class LoginResponse
{
    public string AccessToken { get; set; }
    public DateTime AccessTokenExpirationDateTime { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpirationDateTime { get; set; }
}