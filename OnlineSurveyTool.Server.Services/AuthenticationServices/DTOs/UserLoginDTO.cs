using System.ComponentModel.DataAnnotations;

namespace OnlineSurveyTool.Server.Services.AuthenticationServices.DTOs
{
    public class UserLoginDTO
    {
        [Required]
        [StringLength(32, MinimumLength = 8)]
        public string Login { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
