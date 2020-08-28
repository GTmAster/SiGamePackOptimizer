using System;
using FluentAssertions;
using Xunit;

namespace SiGamePackOptimizer.Tests
{
    public class AssetTypeTests
    {
        [Theory]
        [InlineData(AssetType.Audio, "Audio")]
        [InlineData(AssetType.Image, "Images")]
        [InlineData(AssetType.Video, "Video")]
        public void KnownAssetPrefixConvertsBackAndForth(AssetType assetType, string prefix)
        {
            // act
            var resultPrefix = assetType.EntryPrefix();
            var resultType = prefix.ToAssetType();

            // assert
            resultType.Should().Be(assetType);
            resultPrefix.Should().Be(prefix);
        }

        [Fact]
        public void UnknownAssetPrefix()
        {
            // act
            var resultType = "not-a-prefix".ToAssetType();
            Action prefixAction = () => AssetType.NotAsset.EntryPrefix();

            // assert
            resultType.Should().Be(AssetType.NotAsset);
            prefixAction.Should().Throw<ArgumentException>();
        }
    }
}