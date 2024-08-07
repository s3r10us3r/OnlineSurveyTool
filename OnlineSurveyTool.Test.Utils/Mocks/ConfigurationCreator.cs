using Microsoft.Extensions.Configuration;

namespace OnlineSurveyTool.Test.Utils.Mocks;

public static class ConfigurationCreator
{
    public static IConfiguration MockConfig()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            {"Jwt:Key", "VERY_LONG_MOCK_KEY_TO_GET_ENOUGH_BYTES"},
            {"Jwt:Issuer", "TestIssuer"},
            {"Jwt:Audience", "TestAudience"},
            {"Jwt:RefreshTokenExpiryMinutes", "10080"},
            {"Jwt:AccessTokenExpiryMinutes", "30"},
            {"Settings:TextualQuestionMaximumLength", "1000"}
        };
        
        var builder = new ConfigurationBuilder();
        return builder.AddInMemoryCollection(inMemorySettings!).Build();
    }
}