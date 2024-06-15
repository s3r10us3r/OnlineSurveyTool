using System.ComponentModel.DataAnnotations;

namespace OnlineSurveyTool.Server.DTOs
{
    public class UserDTO
    {
        [Required]
        public string Login { get; set; }
        
        [Required]
        public string EMail { get; set; }
    }
}
