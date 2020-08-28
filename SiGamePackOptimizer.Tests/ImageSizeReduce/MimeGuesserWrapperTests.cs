using System.IO;
using FluentAssertions;
using SiGamePackOptimizer.ImageSizeReduce;
using Xunit;

namespace SiGamePackOptimizer.Tests.ImageSizeReduce
{
    public class MimeGuesserWrapperTests
    {
        private const string TestFilesDir = "TestFiles";

        [Theory]
        [InlineData(@"1000x1503.jpg", true)]
        [InlineData(@"1410x804.jpg", true)]
        [InlineData(@"512x288.png", true)]
        [InlineData(@"archive.siq", false)]
        [InlineData(@"content.xml", false)]
        public void ImageIsCorrectlyDetermined(string path, bool expectedResult)
        {
            // arrange
            var content = File.ReadAllBytes(Path.Combine(TestFilesDir, path));
            var target = new MimeGuesserWrapper();

            // act
            var result = target.IsImage(content);

            // assert
            result.Should().Be(expectedResult);
        }
    }
}
