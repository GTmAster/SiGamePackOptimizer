using HeyRed.Mime;

namespace SiGamePackOptimizer.ImageSizeReduce
{
    public class MimeGuesserWrapper: IMimeWorker
    {
        public bool IsImage(byte[] content)
        {
            var mimeType = MimeGuesser.GuessMimeType(content);
            return mimeType.StartsWith("image");
        }
    }
}