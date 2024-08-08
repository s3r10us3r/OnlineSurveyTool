using OnlineSurveyTool.Server.DAL.Models;

namespace OnlineSurveyTool.Test.Utils.Populators;

public class SurveyPopulator : IPopulator<Survey, string>
{
    public List<Survey> Populate()
    {
        return
        [
            new Survey()
            {
                Name = "Survey 1",
                Id = "survey1",
                IsArchived = false,
                IsOpen = false,
                OwnerId = 1,
                Questions = new QuestionPopulator().Populate()
            },
            
            new Survey()
            {
                Name = "Survey 2",
                Id = "survey2",
                IsArchived = false,
                IsOpen = true,
                OwnerId = 1,
                Questions = new QuestionPopulator().Populate()
            }
        ];
    }
}