using System;
using System.Collections.Generic;
using System.Linq;

namespace SiGamePackOptimizer
{
    public enum AssetType
    {
        Audio,
        Image,
        Video,
        NotAsset
    }

    internal static class AssetTypeExtensions
    {
        private static readonly Dictionary<AssetType, string> Prefixes = new Dictionary<AssetType, string>
        {
            {AssetType.Audio, "Audio"},
            {AssetType.Image, "Images"},
            {AssetType.Video, "Video"}
        };

        public static string EntryPrefix(this AssetType type)
        {
            if (!Prefixes.ContainsKey(type))
                throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown asset type");
            return Prefixes[type];
        }

        public static AssetType ToAssetType(this string prefix)
        {
            return Prefixes.ContainsValue(prefix) 
                ? Prefixes.First(x => x.Value == prefix).Key
                : AssetType.NotAsset;
        }
    }
}