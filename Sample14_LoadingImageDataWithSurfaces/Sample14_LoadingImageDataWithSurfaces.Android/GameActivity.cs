using Android.App;
using Android.Content.PM;
using Ultraviolet;

namespace Sample14_LoadingImageDataWithSurfaces.Android
{
    [Activity(Label = "Sample 14 - Loading Image Data with Surfaces", MainLauncher = true, ConfigurationChanges =
        ConfigChanges.Orientation |
        ConfigChanges.ScreenSize |
        ConfigChanges.KeyboardHidden)]
    public class GameActivity : UltravioletApplication
    {
        public GameActivity() : base("Ultraviolet", "Sample 14 - Loading Image Data with Surfaces")
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
    }
}