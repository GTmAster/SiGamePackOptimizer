using System.Collections.Generic;
using System.Drawing;
using Moq;
using SiGamePackOptimizer.ImageSizeReduce;
using Xunit;

namespace SiGamePackOptimizer.Tests.ImageSizeReduce
{
    public class ImageSizeReducerTests
    {
        private readonly Mock<IImageProcessor> _imageProcessorMock;
        private readonly Mock<ISiGamePack> _packMock;
        private readonly Mock<IMimeWorker> _mimeWorkerMock;
        private readonly ImageSizeReducer _target;
        private readonly ImageSizeReducerSettings _settings;
        private readonly byte[] _imageContent;
        private readonly byte[] _resizedImageContent;
        private const string AssetName = "AssetName";

        public ImageSizeReducerTests()
        {
            _imageContent = new byte[0];
            _resizedImageContent = new byte[0];

            _imageProcessorMock = new Mock<IImageProcessor>();
            _imageProcessorMock
                .Setup(x => x.EncodeImage(_imageContent, It.IsAny<Size>(), It.IsAny<int>()))
                .Returns(_resizedImageContent);
            _settings = new ImageSizeReducerSettings();
            
            _packMock = new Mock<ISiGamePack>();
            _packMock.Setup(x => x.ImageAssets)
                .Returns(new[] {new Asset {Content = _imageContent, Type = AssetType.Image, Name = AssetName}});
            
            _mimeWorkerMock = new Mock<IMimeWorker>();
            _mimeWorkerMock
                .Setup(x => x.IsImage(It.IsAny<byte[]>()))
                .Returns(true);

            _target = new ImageSizeReducer(_imageProcessorMock.Object, _mimeWorkerMock.Object, _settings);
        }

        public static IEnumerable<object[]> TestData =>
            new List<object[]>
            {
                new object[]{new Size(1000, 1503), new Size(1024, 768), new Size(510, 767), 1},
                new object[]{new Size(1410, 804), new Size(1024, 768), new Size(1024, 583), 30},
                new object[]{new Size(512, 288), new Size(1024, 768), new Size(512, 288), 60}
            };

        [Theory]
        [MemberData(nameof(TestData))]
        public void CorrectSizeAndQualityIsPassedToImageProcessor(Size givenSize, Size maxSize, Size expectedSize, int quality)
        {
            // arrange
            _settings.MaximumSize = maxSize;
            _settings.JpegQuality = quality;
            _imageProcessorMock.Setup(x => x.GetImageSize(_imageContent)).Returns(givenSize);

            // act
            _target.Optimize(_packMock.Object);

            // assert
            _imageProcessorMock.Verify(x => x.EncodeImage(_imageContent, expectedSize, quality), Times.Once);
            _packMock.Verify(x => x.InsertAsset(It.Is<Asset>(asset =>
                asset.Content == _resizedImageContent && 
                asset.Name == AssetName && 
                asset.Type == AssetType.Image)));
        }

        [Fact]
        public void NonImagesAreIgnored()
        {
            // arrange
            var imageContent = new byte[0];
            _packMock.Setup(x => x.ImageAssets)
                .Returns(new[] { new Asset { Content = _imageContent, Type = AssetType.Image, Name = AssetName } });
            _mimeWorkerMock.Setup(x => x.IsImage(imageContent)).Returns(false);

            // act
            _target.Optimize(_packMock.Object);

            // assert
            _imageProcessorMock.Verify(x => x.GetImageSize(It.IsAny<byte[]>()), Times.Never);
            _imageProcessorMock.Verify(x => x.EncodeImage(It.IsAny<byte[]>(), It.IsAny<Size>(), It.IsAny<int>()),
                Times.Never);
            _packMock.Verify(x => x.InsertAsset(It.IsAny<Asset>()), Times.Never);
        }
    }
}