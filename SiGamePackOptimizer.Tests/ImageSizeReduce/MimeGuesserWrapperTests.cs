using System.IO;
using FluentAssertions;
using SiGamePackOptimizer.ImageSizeReduce;
using Xunit;

namespace SiGamePackOptimizer.Tests.ImageSizeReduce
{
    public class MimeGuesserWrapperTests
    {
        [Theory]
        [InlineData(@"TestFiles\1000x1503.jpg", true)]
        [InlineData(@"TestFiles\1410x804.jpg", true)]
        [InlineData(@"TestFiles\512x288.png", true)]
        [InlineData(@"TestFiles\archive.siq", false)]
        [InlineData(@"TestFiles\content.xml", false)]
        public void ImageIsCorrectlyDetermined(string path, bool expectedResult)
        {
            // arrange
            var content = File.ReadAllBytes(path);
            var target = new MimeGuesserWrapper();

            // act
            var result = target.IsImage(content);
            
            // assert
            result.Should().Be(expectedResult);
        }
    }
}
