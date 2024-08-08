namespace OnlineSurveyTool.Test.Utils.Populators;

public interface IPopulator<T, TId>
{
    List<T> Populate();
}