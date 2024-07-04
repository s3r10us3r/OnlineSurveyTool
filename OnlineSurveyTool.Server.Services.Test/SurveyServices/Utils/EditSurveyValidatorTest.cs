using OnlineSurveyTool.Server.Services.SurveyServices.Utils;
using OnlineSurveyTool.Server.Services.SurveyServices.Utils.Interfaces;
using OnlineSurveyTool.Server.Services.Test.Mocks;

namespace OnlineSurveyTool.Server.Services.Test.SurveyServices.Utils;

public class EditSurveyValidatorTest
{
    private IEditSurveyValidator _editSurveyValidator;

    [SetUp]
    public void SetUp()
    {
        var qValidator = new QuestionValidator(ConfigurationCreator.MockConfig());
        var guidGenerator = new GuidGenerator();
        var cOConverter = new ChoiceOptionConverter(guidGenerator);
        var questionConverter = new QuestionConverter(guidGenerator, cOConverter);
        var sConverter = new SurveyConverter(questionConverter, guidGenerator);
        var sValidator = new SurveyValidator(qValidator);
        _editSurveyValidator = new EditSurveyValidator(qValidator, sConverter, sValidator);
    }
    
    
}