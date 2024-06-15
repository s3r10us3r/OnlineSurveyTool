using System.ComponentModel.DataAnnotations;

namespace OnlineSurveyTool.Server.Services.DTOs
{
    public class UserRegisterDTO
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string EMail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
