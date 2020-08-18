using Moq;
using Xunit;

namespace SiGamePackOptimizer.Tests
{
    public class SiGamePackOptimizerAppTests
    {
        private readonly Mock<ISiGamePack> _packMock;
        private readonly Mock<IOptimizer> _optimizerMock1, _optimizerMock2, _optimizerMock3;
        private readonly SiGamePackOptimizerApp _target;

        public SiGamePackOptimizerAppTests()
        {
            _packMock = new Mock<ISiGamePack>();
            _optimizerMock1 = new Mock<IOptimizer>();
            _optimizerMock2 = new Mock<IOptimizer>();
            _optimizerMock3 = new Mock<IOptimizer>();
            
        }

        [Fact]
        public void AppInvokesAllOptimizers()
        {
            // arrange
            const string inputPath = "inputPath";
            const string outputPath = "outputPath";
            var target = new SiGamePackOptimizerApp(_packMock.Object,
                new[] { _optimizerMock1.Object, _optimizerMock2.Object, _optimizerMock3.Object }, inputPath, outputPath);

            // act
            target.Run();

            // assert
            _packMock.Verify(x => x.Open(inputPath), Times.Once);
            _optimizerMock1.Verify(x => x.Optimize(_packMock.Object), Times.Once);
            _optimizerMock2.Verify(x => x.Optimize(_packMock.Object), Times.Once);
            _optimizerMock3.Verify(x => x.Optimize(_packMock.Object), Times.Once);
            _packMock.Verify(x => x.Save(outputPath), Times.Once);
        }
    }
}
