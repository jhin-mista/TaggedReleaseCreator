using FluentAssertions;
using ReleaseCreator.Client.Util;

namespace ReleaseCreator.Client.Tests.Util;

[TestFixture]
public class FileServiceTest
{
    private string _tmpFilePath;

    [SetUp]
    public void SetUp()
    {
        _tmpFilePath = Path.GetTempFileName();
    }

    [TearDown]
    public void TearDown()
    {
        File.Delete(_tmpFilePath);
    }

    [Test]
    public void AppendLine_ShouldAppendSingleLine()
    {
        // arrange
        var content = "content";
        var sut = new FileService();

        // act
        sut.AppendLine(_tmpFilePath, content);

        // assert
        var lines = File.ReadAllLines(_tmpFilePath);

        lines.Should().ContainSingle().Which.Should().Be(content);
    }
}