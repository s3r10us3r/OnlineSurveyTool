using OnlineSurveyTool.Server.DAL.Interfaces;
using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.Interfaces
{
    public interface IService<E, R> where E: EntityBase, new() where R : IRepo<E>
    {
    }
}
