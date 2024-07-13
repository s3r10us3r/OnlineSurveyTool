using System.ComponentModel.DataAnnotations;

namespace OnlineSurveyTool.Server.DAL.Models;

public abstract class EntityBase<TId>
{
    [Key]
    public virtual TId Id { get; set; }
}