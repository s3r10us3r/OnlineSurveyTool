using System.ComponentModel.DataAnnotations;

namespace OnlineSurveyTool.Server.Services.AuthenticationServices.DTOs
{
    public class UserDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
