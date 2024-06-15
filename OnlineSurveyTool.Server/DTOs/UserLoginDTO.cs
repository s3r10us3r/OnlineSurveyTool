using System.ComponentModel.DataAnnotations;

namespace OnlineSurveyTool.Server.DTOs
{
    public class UserLoginDTO
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
