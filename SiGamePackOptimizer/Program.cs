using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Autofac;
using CommandLine;
using Serilog;
using SiGamePackOptimizer.ImageSizeReduce;

[assembly:InternalsVisibleTo("SiGamePackOptimizer.Tests")]
[assembly:InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace SiGamePackOptimizer
{
    internal static class Program
    {
        private static IContainer _container;

        private static int Main(string[] args)
        {
            return Parser.Default
                .ParseArguments<Options>(args)
                .MapResult(Run, _ => 1);
        }

        private static int Run(Options options)
        {
            ConfigureLogger();
            ConfigureContainer(options);

            using var scope = _container.BeginLifetimeScope();
            var app = _container.Resolve<SiGamePackOptimizerApp>();
            app.Run();

            return 0;
        }

        private static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
#else
                .MinimumLevel.Information()
#endif           
                .WriteTo.Console()
                .CreateLogger();
        }

        private static void ConfigureContainer(Options options)
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));

            var builder = new ContainerBuilder();

            builder.RegisterType<SiGamePackOptimizerApp>()
                .WithParameter("inputFilepath", options.InputFile)
                .WithParameter("outputFilepath", options.OutputFile);
            builder.RegisterType<ZipFileWrapper>()
                .As<IZipFile>()
                .WithParameter("tempFile", tempFolder);
            builder.RegisterType<MagickImageProcessor>().As<IImageProcessor>();
            builder.RegisterType<SiGamePack>().As<ISiGamePack>();
            builder.RegisterType<MimeGuesserWrapper>().As<IMimeWorker>();

            if (options.Optimizers.Contains(Optimizer.ImageSizeReducer))
                builder.RegisterType<ImageSizeReducer>()
                    .As<IOptimizer>()
                    .WithParameter("settings", new ImageSizeReducerSettings
                    {
                        MaximumSize = new Size(options.ImageSizeReducerMaximumWidth,
                            options.ImageSizeReducerMaximumHeight),
                        JpegQuality = options.ImageSizeReducerJpegQuality
                    });

            _container = builder.Build();
        }
    }
}
