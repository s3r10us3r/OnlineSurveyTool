using System.ComponentModel.DataAnnotations;

namespace OnlineSurveyTool.Server.Services.DTOs
{
    public class UserRegisterDTO
    {
        [Required]
        [StringLength(32, MinimumLength = 8)]
        public string Login { get; set; }
        [Required]  
        [StringLength(100)]
        public string EMail { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
