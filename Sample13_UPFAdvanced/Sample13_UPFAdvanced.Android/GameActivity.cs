using Android.App;
using Android.Content.PM;
using Ultraviolet;

namespace Sample13_UPFAdvanced.Android
{
    [Activity(Label = "Sample 13 - UPF (Advanced)", MainLauncher = true, ConfigurationChanges =
        ConfigChanges.Orientation |
        ConfigChanges.ScreenSize |
        ConfigChanges.KeyboardHidden)]
    public class GameActivity : UltravioletApplication
    {
        public GameActivity() : base("Ultraviolet", "Sample 13 - UPF (Advanced)")
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