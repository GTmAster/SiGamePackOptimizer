using System;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using Xunit;

namespace SiGamePackOptimizer.Tests
{
    public class SiGamePackTests
    {
        private readonly Mock<IZipFile> _zipFileMock;
        private readonly SiGamePack _target;

        public SiGamePackTests()
        {
            _zipFileMock = new Mock<IZipFile>();
            _zipFileMock
                .Setup(x => x.Entries)
                .Returns(new[]
                    {@"Audio\1.mp3", @"Audio\2.mp3", @"Images\1.jpg", @"Images\2.png", @"Video\1.mpg", @"Video\2.mpg"});
            _zipFileMock.Setup(x => x.GetEntryContent(It.IsAny<string>())).Returns(new byte[0]);

            _target = new SiGamePack(_zipFileMock.Object);
        }

        [Fact]
        public void OpenUnpacksFile()
        {
            // arrange
            const string filename = "File";

            // act
            _target.Open(filename);

            // arrange
            _zipFileMock.Verify(x => x.Unpack(filename), Times.Once);
        }

        [Fact]
        public void SavePackTheFile()
        {
            // arrange
            const string filename = "File";

            // act
            _target.Save(filename);

            // arrange
            _zipFileMock.Verify(x => x.Pack(filename), Times.Once);
        }

        [Fact]
        public void AssetsReturnsAll()
        {
            // act
            var result = _target.Assets.ToArray();

            // arrange
            result.Should()
                .HaveCount(6)
                .And.Contain(CorrectAsset("1.mp3", AssetType.Audio))
                .And.Contain(CorrectAsset("2.mp3", AssetType.Audio))
                .And.Contain(CorrectAsset("1.jpg", AssetType.Image))
                .And.Contain(CorrectAsset("2.png", AssetType.Image))
                .And.Contain(CorrectAsset("1.mpg", AssetType.Video))
                .And.Contain(CorrectAsset("2.mpg", AssetType.Video));
        }

        [Fact]
        public void ImageAssetsReturnsOnlyImages()
        {
            // act
            var result = _target.ImageAssets.ToArray();

            // arrange
            result.Should()
                .HaveCount(2)
                .And.Contain(CorrectAsset("1.jpg", AssetType.Image))
                .And.Contain(CorrectAsset("2.png", AssetType.Image));
        }

        private static Expression<Func<Asset, bool>> CorrectAsset(string name, AssetType type)
        {
            return asset => asset.Name == name && asset.Type == type;
        }

        [Theory]
        [InlineData("1.jpg", AssetType.Image, @"Images/1.jpg")]
        [InlineData("1.mp3", AssetType.Audio, @"Audio/1.mp3")]
        [InlineData("1.mpg", AssetType.Video, @"Video/1.mpg")]
        public void InsertAssetInvokesZipFile(string name, AssetType type, string expectedPath)
        {
            // arrange
            var asset = new Asset { Name = name, Type = type, Content = new byte[0]};

            // act
            _target.InsertAsset(asset);

            // assert
            _zipFileMock.Verify(x => x.InsertEntry(expectedPath, asset.Content));
        }
    }
}