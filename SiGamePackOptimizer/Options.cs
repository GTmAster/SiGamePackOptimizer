using System;
using System.Collections.Generic;
using CommandLine;

namespace SiGamePackOptimizer
{
    public class Options
    {
        [Option('z', "optimizers", Required = true, Min = 1, 
            HelpText = "Optimizers to be used, comma separated. Available optimizers: ImageSizeReducer")]
        public IEnumerable<Optimizer> Optimizers { get; set; }

        [Option('i', "input", Required = true, HelpText = "Path to input file")]
        public string InputFile { get; set; }

        [Option('o', "output", Required = true, HelpText = "Path to output file")]
        public string OutputFile { get; set; }

        #region ImageSizeReducer
        private int _imageSizeReducerMaximumWidth;
        private int _imageSizeReducerMaximumHeight;
        private int _imageSizeReducerJpegQuality;


        [Option("imagemaxwidth", Required = false, Default = 1024,
            HelpText =
                "Maximum width of image. All images large then provided width will be resized preserving aspect ratio")]
        public int ImageSizeReducerMaximumWidth
        {
            get => _imageSizeReducerMaximumWidth;
            set
            {
                if (value < 1)
                    throw new ArgumentException("Maximum width should be more than 0");
                _imageSizeReducerMaximumWidth = value;
            }
        }

        [Option("imagemaxheight", Required = false, Default = 768,
            HelpText =
                "Maximum height of image. All images large then provided height will be resized preserving aspect ratio")]
        public int ImageSizeReducerMaximumHeight
        {
            get => _imageSizeReducerMaximumHeight;
            set
            {
                if (value < 1)
                    throw new ArgumentException("Maximum height should be more than 0");
                _imageSizeReducerMaximumHeight = value;
            }
        }

        [Option("imagejpegquality", Required = false, Default = 75,
            HelpText =
                "Jpeg quality for images. From 0 to 100. Larger number - better quality and larger size. All images will be encoded in JPEG format with given quality")]
        public int ImageSizeReducerJpegQuality
        {
            get => _imageSizeReducerJpegQuality;
            set
            {
                if (value < 1 || value > 100)
                    throw new ArgumentException("Jpeg quality should be in range 0-100");
                _imageSizeReducerJpegQuality = value;
            }
        }

        #endregion
    }
}