using Android.App;
using Android.Content.PM;
using Ultraviolet;

namespace Sample4_ContentManagement.Android
{
    [Activity(Label = "Sample 4 - Content Management", MainLauncher = true, ConfigurationChanges =
        ConfigChanges.Orientation |
        ConfigChanges.ScreenSize |
        ConfigChanges.KeyboardHidden)]
    public class GameActivity : UltravioletApplication
    {
        public GameActivity() : base("Ultraviolet", "Sample 4 - Content Management")
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