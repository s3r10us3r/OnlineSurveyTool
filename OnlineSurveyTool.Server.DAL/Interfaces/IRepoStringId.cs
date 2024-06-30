using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.DAL.Interfaces;

public interface IRepoStringId<T> : IBaseRepo<T, string> where T : EntityBaseStringId, new() 
{
}