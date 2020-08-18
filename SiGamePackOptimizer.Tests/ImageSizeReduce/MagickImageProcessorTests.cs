using System.Drawing;
using System.IO;
using FluentAssertions;
using SiGamePackOptimizer.ImageSizeReduce;
using Xunit;

namespace SiGamePackOptimizer.Tests.ImageSizeReduce
{
    public class MagickImageProcessorTests
    {
        private readonly MagickImageProcessor _target;

        public MagickImageProcessorTests()
        {
            _target = new MagickImageProcessor();
        }

        [Theory]
        [InlineData(@"TestFiles\1000x1503.jpg", 1000, 1503)]
        [InlineData(@"TestFiles\1410x804.jpg", 1410, 804)]
        [InlineData(@"TestFiles\512x288.png", 512, 288)]
        public void ImageSizeIsReturned(string testFilename, int expectedWidth, int expectedHeight)
        {
            // arrange
            var fileContent = File.ReadAllBytes(testFilename);

            // act
            var result = _target.GetImageSize(fileContent);

            // assert
            result.Width.Should().Be(expectedWidth);
            result.Height.Should().Be(expectedHeight);
        }

        [Theory]
        [InlineData(@"TestFiles\1000x1503.jpg", 800, 600)]
        [InlineData(@"TestFiles\1410x804.jpg", 100, 100)]
        [InlineData(@"TestFiles\512x288.png", 200, 300)]
        public void ImageIsResized(string testFilename, int width, int height)
        {
            // arrange
            var fileContent = File.ReadAllBytes(testFilename);

            // act
            var result = _target.EncodeImage(fileContent, new Size(width, height), 50);

            // assert
            var newSize = _target.GetImageSize(result);
            newSize.Width.Should().Be(width);
            newSize.Height.Should().Be(height);
        }
    }
}
