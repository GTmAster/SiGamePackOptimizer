using System;
using FluentAssertions;
using Xunit;

namespace SiGamePackOptimizer.Tests
{
    public class OptionsTests
    {
        [Fact]
        public void ImageSizeReducerOptionsValidation()
        {
            // arrange
            var target = new Options();

            // act
            Action maxWidthAct = () => target.ImageSizeReducerMaximumWidth = 0;
            Action maxHeightAct = () => target.ImageSizeReducerMaximumHeight = 0;
            Action jpegQualityTooHighAct = () => target.ImageSizeReducerJpegQuality = 0;
            Action jpegQualityTooLowAct = () => target.ImageSizeReducerJpegQuality = 101;

            // assert
            maxWidthAct.Should().Throw<ArgumentException>();
            maxHeightAct.Should().Throw<ArgumentException>();
            jpegQualityTooHighAct.Should().Throw<ArgumentException>();
            jpegQualityTooLowAct.Should().Throw<ArgumentException>();
        }
    }
}