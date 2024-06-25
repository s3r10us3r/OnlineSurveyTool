using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyServices.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers;
using OnlineSurveyTool.Server.Services.SurveyServices.Helpers.Interfaces;

namespace OnlineSurveyTool.Server.Services.Test.SurveyServices.Helpers;

[TestFixture]
public class QuestionFactoryTest
{
    private IQuestionFactory _questionFactory;
    
    [OneTimeSetUp]
    public void SetUp()
    {
        _questionFactory = new QuestionFactory();
    }

    private void ShouldCorrectlyConvertQuestionToType<T>(Question question) where T : QuestionBase
    {
        var converted = _questionFactory.MakeQuestionBase(question);
        Assert.That(converted, Is.TypeOf<T>());
    }
}