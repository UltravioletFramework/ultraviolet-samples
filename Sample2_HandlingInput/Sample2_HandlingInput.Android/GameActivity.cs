using Android.App;
using Android.Content.PM;
using Sample2_HandlingInput.Shared;
using Ultraviolet;

namespace Sample2_HandlingInput.Android
{
    [Activity(Label = "Sample 2 - Handling Input", MainLauncher = true, ConfigurationChanges =
        ConfigChanges.Orientation |
        ConfigChanges.ScreenSize |
        ConfigChanges.KeyboardHidden)]
    public class GameActivity : UltravioletApplication
    {
        public GameActivity() : base("Ultraviolet", "Sample 2 - Handling Input")
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