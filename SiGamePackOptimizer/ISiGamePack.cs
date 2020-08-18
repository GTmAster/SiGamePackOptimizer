using System.Collections.Generic;

namespace SiGamePackOptimizer
{
    internal interface ISiGamePack
    {
        void Open(string path);
        void Save(string path);
        IEnumerable<Asset> Assets { get; }
        IEnumerable<Asset> ImageAssets { get; }
        void InsertAsset(Asset asset);
    }
}