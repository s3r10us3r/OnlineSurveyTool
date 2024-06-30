using System.ComponentModel.DataAnnotations;

namespace OnlineSurveyTool.Server.DAL.Models;

public abstract class EntityBase<TId>
{
    [Key]
    public TId Id { get; set; }
}