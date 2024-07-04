using OnlineSurveyTool.Server.Services.SurveyServices.Utils;

namespace OnlineSurveyTool.Server.Services.Test.SurveyServices.Utils;

[TestFixture]
public class GuidGeneratorTest
{
    private IGuidGenerator _guidGenerator;

    [SetUp]
    public void SetUp()
    {
        _guidGenerator = new GuidGenerator();
    }

    [Test]
    public void ShouldBe36CharLong()
    {
        var guid = _guidGenerator.GenerateGuid();
        Assert.That(guid, Has.Length.EqualTo(36));
    }

    [Test]
    public void ShouldBeUnique()
    {
        var guid1 = _guidGenerator.GenerateGuid();
        var guid2 = _guidGenerator.GenerateGuid();
        
        Console.WriteLine("{0} {1}", guid1, guid2);
        Assert.That(guid1, Is.Not.EqualTo(guid2));
    }
}