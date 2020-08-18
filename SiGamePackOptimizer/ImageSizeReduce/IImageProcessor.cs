using System.Drawing;

namespace SiGamePackOptimizer.ImageSizeReduce
{
    internal interface IImageProcessor
    {
        byte[] EncodeImage(byte[] imageData, Size size, int jpegQuality);
        Size GetImageSize(byte[] imageData);
    }
}