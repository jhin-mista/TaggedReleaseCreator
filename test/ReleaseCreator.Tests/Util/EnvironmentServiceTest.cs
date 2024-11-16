using FluentAssertions;
using ReleaseCreator.CommandLine.Util;

namespace ReleaseCreator.CommandLine.Tests.Util
{
    [TestFixture]
    public class EnvironmentServiceTest
    {
        private EnvironmentService _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new();
        }

        [Test]
        public void GetEnvironmentVariable_ShouldReturnExpectedValue()
        {
            // arrange
            var key = Guid.NewGuid().ToString();
            var value = "test";

            Environment.SetEnvironmentVariable(key, value);

            // act
            var result = _sut.GetEnvironmentVariable(key);

            // assert
            result.Should().Be(value);
        }

        [Test]
        public void GetEnvironmentVariable_WhenVariableDoesNotExist_ShouldReturnNull()
        {
            // arrange
            var key = Guid.NewGuid().ToString();

            // act
            var result = _sut.GetEnvironmentVariable(key);

            // assert
            result.Should().BeNull();
        }
    }
}