using System.ComponentModel.DataAnnotations;

namespace OnlineSurveyTool.Server.DAL.Models
{
    public abstract class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}
