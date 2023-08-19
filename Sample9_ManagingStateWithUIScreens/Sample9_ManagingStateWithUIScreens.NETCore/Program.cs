using Ultraviolet;

namespace Sample9_ManagingStateWithUIScreens.NETCore
{
    public class Program : UltravioletApplication
    {
        public Program() : base("Ultraviolet", "Sample 9 - Managing State with UI Screens")
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
    }
}