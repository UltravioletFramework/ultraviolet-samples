using Ultraviolet;

namespace Sample16_CustomTextLayoutCommands.NETCore
{
    public class Program : UltravioletApplication
    {
        public Program() : base("Ultraviolet", "Sample 16 - Custom Text Layout Commands")
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