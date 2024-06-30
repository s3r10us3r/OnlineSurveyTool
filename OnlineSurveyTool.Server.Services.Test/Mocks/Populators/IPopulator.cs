using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Server.Services.Test.Mocks;

public interface IPopulator<T, TId> where T : EntityBase<TId>
{
    List<T> Populate();
}