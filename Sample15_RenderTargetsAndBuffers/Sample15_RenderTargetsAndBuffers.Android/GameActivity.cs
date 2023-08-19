using System;
using System.IO;
using Android.App;
using Android.Content.PM;
using Android.Media;
using Android.Webkit;
using Sample15_RenderTargetsAndBuffers.Shared;
using Ultraviolet;
using Ultraviolet.Graphics;
using AndroidEnvironment = Android.OS.Environment;

namespace Sample15_RenderTargetsAndBuffers.Android
{
	[Activity(Label = "Sample 15 - Render Targets and Buffers", MainLauncher = true, ConfigurationChanges =
        ConfigChanges.Orientation |
        ConfigChanges.ScreenSize |
        ConfigChanges.KeyboardHidden)]
    public class GameActivity : UltravioletApplication, IImageSaver
    {
        public GameActivity() : base("Ultraviolet", "Sample 15 - Render Targets and Buffers")
        {
        }

        protected override void OnInitialized()
        {
            UsePlatformSpecificFileSource();
            base.OnInitialized();
        }

        protected override UltravioletApplicationAdapter OnCreatingApplicationAdapter()
        {
            return new Game(this);
        }

        public void SaveImage(SurfaceSaver surfaceSaver, RenderTarget2D target, out double messageOpacity, out string message)
        {
            var filename = $"output-{DateTime.Now:yyyyMMdd-HHmmss}.png";

            var dir = AndroidEnvironment.GetExternalStoragePublicDirectory(
                AndroidEnvironment.DirectoryPictures).AbsolutePath;
            var path = Path.Combine(dir, filename);

            using (var stream = File.OpenWrite(path))
                surfaceSaver.SaveAsPng(target, stream);

            MediaScannerConnection.ScanFile(ApplicationContext, new String[] { path },
                new String[] { MimeTypeMap.Singleton.GetMimeTypeFromExtension("png") }, null);

            message = $"Image saved to photo gallery";
            messageOpacity = 1;
        }
    }
}