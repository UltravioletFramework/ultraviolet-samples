using Ultraviolet;

namespace Sample8_PlayingSoundEffects.NETCore
{
    public class Program : UltravioletApplication
    {
        public Program() : base("Ultraviolet", "Sample 8 - Playing Sound Effects")
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