using Sample15_RenderTargetsAndBuffers.Shared;
using System;
using System.IO;
using Ultraviolet;
using Ultraviolet.Graphics;

namespace Sample15_RenderTargetsAndBuffers.NETCore
{
    public class Program : UltravioletApplication, IImageSaver
    {
        public Program() : base("Ultraviolet", "Sample 15 - Render Targets and Buffers")
        {
        }

        public static void Main(string[] args)
        {
            using (var app = new Program())
            {
                app.Run();
            }
        }

        protected override UltravioletApplicationAdapter OnCreatingApplicationAdapter()
        {
            return new Game(this);
        }

        protected override void OnInitialized()
        {
            UsePlatformSpecificFileSource();
            base.OnInitialized();
        }

        public void SaveImage(SurfaceSaver surfaceSaver, RenderTarget2D target, out double messageOpacity, out string message)
        {
            var filename = $"output-{DateTime.Now:yyyyMMdd-HHmmss}.png";
            var path = filename;

            using (var stream = File.OpenWrite(path))
                surfaceSaver.SaveAsPng(target, stream);

            message = $"Image saved to {filename}";
            messageOpacity = 1;
        }
    }
}