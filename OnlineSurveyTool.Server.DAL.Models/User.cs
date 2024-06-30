using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineSurveyTool.Server.DAL.Models
{
    [Table("Users")]
    [Index(nameof(Login))]
    public class User : EntityBaseIntegerId
    {
        [Required]
        [StringLength(32, MinimumLength = 8)]
        public string Login { get; set; }

        [Required]
        [StringLength(100)]
        public string EMail { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        public virtual IEnumerable<Survey> Surveys { get; set; }
    }
}
