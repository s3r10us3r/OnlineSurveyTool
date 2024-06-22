using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.Services.SurveyServices.Interfaces;

namespace OnlineSurveyTool.Server.Services.SurveyServices;

public class SurveyCreationService: ISurveyCreationService
{
    private readonly ISurveyRepo _surveyRepo;
    private readonly IConfiguration _config;
    
    private static readonly char[] chars =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
    
    
    public SurveyCreationService(ISurveyRepo surveyRepo, IConfiguration config)
    {
        _surveyRepo = surveyRepo;
        _config = config;
    }
    
    
    private string GenerateSurveyID()
    {
        while (true)
        {
            int size = int.Parse(_config["Settings:SurveyIDLength"]!);
            byte[] data = new byte[4 * size];
            using (var crypto = RandomNumberGenerator.Create())
            {
                crypto.GetBytes(data);
            }

            StringBuilder result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            var id = result.ToString();
            if (_surveyRepo.GetOne(id) is null)
            {
                return result.ToString();
            }
        }
    }
    
    
}