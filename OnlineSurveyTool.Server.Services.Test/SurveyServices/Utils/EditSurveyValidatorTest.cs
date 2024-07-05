using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.SurveyService.DTOs;
using OnlineSurveyTool.Server.Services.SurveyServices.Utils;
using OnlineSurveyTool.Server.Services.SurveyServices.Utils.Interfaces;
using OnlineSurveyTool.Server.Services.Test.Mocks;
using OnlineSurveyTool.Server.Services.Test.Mocks.Populators;

namespace OnlineSurveyTool.Server.Services.Test.SurveyServices.Utils;

public class EditSurveyValidatorTest
{
    private IEditSurveyValidator _editSurveyValidator;
    private Survey _survey;
    private SurveyEditDto _dto;

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
        _survey = new SurveyPopulator().Populate()[0];
        _dto = new SurveyEditDto()
        {
            Id = _survey.Id,
            Name = "New survey name",
            DeletedQuestions = ["question5"],
            NewQuestions =
            [
                new()
                {
                    Value = "New question",
                    Number = 3,
                    Maximum = 10,
                    Minimum = 1,
                    CanBeSkipped = true,
                    Type = "Numerical Integer"
                }
            ],
            EditedQuestions =
            [
                new()
                {
                    Id = "question4",
                    Number = 4,
                }
            ]
        };
    }

    [Test]
    public void ShouldValidateAValidRequest()
    {
        var result = _editSurveyValidator.ValidateSurveyEdit(_dto, _survey, out var message);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);
            Assert.That(message, Is.Empty);
        });
    }

    [Test]
    public void ShouldInvalidateARequestWithNumberViolation()
    {
        _dto.EditedQuestions![0].Number = 1;
        var result = _editSurveyValidator.ValidateSurveyEdit(_dto, _survey, out var message);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(message, Is.Not.Empty);
        });
    }

    [Test]
    public void ShouldInvalidateARequestWithDeleteWithNonExistentId()
    {
        _dto.DeletedQuestions!.Add("noid");
        var result = _editSurveyValidator.ValidateSurveyEdit(_dto, _survey, out var message);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(message, Is.Not.Empty);
        });
    }

    [Test]
    public void ShouldInvalidateARequestWithEditWithNonExistentId()
    {
        _dto.EditedQuestions![0].Id = "noid";
        var result = _editSurveyValidator.ValidateSurveyEdit(_dto, _survey, out var message);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(message, Is.Not.Empty);
        });
    }

    [Test]
    public void ShouldInvalidateARequestWihInvalidNewQuestion()
    {
        _dto.NewQuestions!.Add(
            new()
            {
                CanBeSkipped = false,
                Maximum = -1000,
                Minimum = -19,
                Number = 5,
                Value = "New question",
                Type = "Numerical Integer"
            }
        );
        var result = _editSurveyValidator.ValidateSurveyEdit(_dto, _survey, out var message);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.False);
            Assert.That(message, Is.Not.Empty);
        });
    }
}
