using System.ComponentModel.DataAnnotations;

namespace OnlineSurveyTool.Server.DAL.Models;

public class EntityBaseStringId : EntityBase<string>
{
    [Key]
    [StringLength(36, MinimumLength = 36)]
    public new string Id { get; set; }
}