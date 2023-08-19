using Android.App;
using Android.Content.PM;
using static Java.Util.Jar.Attributes;
using Ultraviolet;

namespace Sample1_CreatingAnApplication.Android
{
    [Activity(Label = "Sample 1 - Creating an Application", MainLauncher = true, ConfigurationChanges =
        ConfigChanges.Orientation |
        ConfigChanges.ScreenSize |
        ConfigChanges.KeyboardHidden)]
    public class GameActivity : UltravioletApplication
    {
        public GameActivity() : base("Ultraviolet", "Sample 1 - Creating an Application")
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