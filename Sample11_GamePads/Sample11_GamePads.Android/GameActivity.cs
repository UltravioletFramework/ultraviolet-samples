using Android.App;
using Android.Content.PM;
using Ultraviolet;

namespace Sample11_GamePads.Android
{
    [Activity(Label = "Sample 11 - Game Pads", MainLauncher = true, ConfigurationChanges =
        ConfigChanges.Orientation |
        ConfigChanges.ScreenSize |
        ConfigChanges.KeyboardHidden)]
    public class GameActivity : UltravioletApplication
    {
        public GameActivity() : base("Ultraviolet", "Sample 11 - Game Pads")
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