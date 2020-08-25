using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SiGamePackOptimizer
{
    internal class SiGamePack: ISiGamePack
    {
        private readonly IZipFile _zipFile;

        public SiGamePack(IZipFile zipFile)
        {
            _zipFile = zipFile;
        }

        public void Open(string path)
        {
            _zipFile.Unpack(path);
        }

        public void Save(string path)
        {
            _zipFile.Pack(path);
        }

        public IEnumerable<Asset> Assets 
            => _zipFile.Entries.Select(ConvertEntryToAsset);

        public IEnumerable<Asset> ImageAssets
            => Assets.Where(x => x.Type == AssetType.Image);

        public void InsertAsset(Asset asset)
        {
            var entry = $"{asset.Type.EntryPrefix()}/{asset.Name}";
            _zipFile.InsertEntry(entry, asset.Content);
        }

        private Asset ConvertEntryToAsset(string entry)
        {
            var segments = entry.Split('/');

            var prefix = string.Empty;
            var name = entry;
            if (segments.Length == 2)
            {
                prefix = segments[0];
                name = segments[1];
            }

            var content = _zipFile.GetEntryContent(entry);

            return new Asset {Name = name, Type = prefix.ToAssetType(), Content = content};
        }
    }
}