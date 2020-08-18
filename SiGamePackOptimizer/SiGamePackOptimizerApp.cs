using System.Collections.Generic;
using Serilog;

namespace SiGamePackOptimizer
{
    internal class SiGamePackOptimizerApp
    {
        private readonly ISiGamePack _pack;
        private readonly IEnumerable<IOptimizer> _optimizers;
        private readonly string _inputFilepath;
        private readonly string _outputFilepath;

        public SiGamePackOptimizerApp(ISiGamePack pack, IEnumerable<IOptimizer> optimizers, string inputFilepath, string outputFilepath)
        {
            _pack = pack;
            _optimizers = optimizers;
            _inputFilepath = inputFilepath;
            _outputFilepath = outputFilepath;
        }

        public void Run()
        {
            Log.Information("SiGamePackOptimizer started");
            _pack.Open(_inputFilepath);
            foreach (var optimizer in _optimizers)
            {
                optimizer.Optimize(_pack);
            }
            _pack.Save(_outputFilepath);
            Log.Information("SiGamePackOptimizer finished");
        }
    }
}