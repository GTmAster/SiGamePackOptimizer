using System.Drawing;
using System.IO;
using ImageMagick;

namespace SiGamePackOptimizer.ImageSizeReduce
{
    internal class MagickImageProcessor: IImageProcessor
    {
        public byte[] EncodeImage(byte[] imageData, Size size, int jpegQuality)
        {
            using var image = new MagickImage(imageData);
            image.Resize(new MagickGeometry { Width = size.Width, Height = size.Height, IgnoreAspectRatio = true, FillArea = false});
            image.Format = MagickFormat.Jpeg;
            image.Quality = jpegQuality;
            using var ms = new MemoryStream();
            image.Write(ms);
            return ms.ToArray();
        }

        public Size GetImageSize(byte[] imageData)
        {
            using var image = new MagickImage(imageData);
            return new Size(image.Width, image.Height);
        }
    }
}
