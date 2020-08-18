using System;
using System.Drawing;
using System.Linq;
using Serilog;
using Serilog.Core;

namespace SiGamePackOptimizer.ImageSizeReduce
{
    internal class ImageSizeReducer: IOptimizer
    {
        private readonly IImageProcessor _imageProcessor;
        private readonly IMimeWorker _mimeWorker;
        private readonly ImageSizeReducerSettings _settings;

        public ImageSizeReducer(IImageProcessor imageProcessor, IMimeWorker mimeWorker, ImageSizeReducerSettings settings)
        {
            _imageProcessor = imageProcessor;
            _mimeWorker = mimeWorker;
            _settings = settings;
        }

        public void Optimize(ISiGamePack pack)
        {
            var assets = pack.ImageAssets.ToArray();
            var n = assets.Length;
            Log.Information($"ImageSizeReducer started. {n} assets to optimize");
            for (var i = 0; i < assets.Length; i++)
            {
                Log.Information($"ImageSizeReducer {i+1} / {n}: {assets[i].Name}");
                if (!_mimeWorker.IsImage(assets[i].Content)) continue;
                var originalSize = _imageProcessor.GetImageSize(assets[i].Content);
                var newSize = CalculateNewSize(originalSize);
                var newAsset = new Asset
                {
                    Name = assets[i].Name,
                    Type = assets[i].Type,
                    Content = _imageProcessor.EncodeImage(assets[i].Content, newSize, _settings.JpegQuality)
                };
                pack.InsertAsset(newAsset);
            }
        }

        private Size CalculateNewSize(Size originalSize)
        {
            var widthCoefficient = (double)_settings.MaximumSize.Width / originalSize.Width;
            var heightCoefficient = (double)_settings.MaximumSize.Height / originalSize.Height;
            var resultCoefficient = Math.Min(widthCoefficient, heightCoefficient);
            return resultCoefficient >= 1.0
                ? originalSize 
                : new Size((int)(originalSize.Width * resultCoefficient), (int)(originalSize.Height * resultCoefficient));
        }
    }
}
