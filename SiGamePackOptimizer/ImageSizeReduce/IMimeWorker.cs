namespace SiGamePackOptimizer.ImageSizeReduce
{
    public interface IMimeWorker
    {
        bool IsImage(byte[] content);
    }
}