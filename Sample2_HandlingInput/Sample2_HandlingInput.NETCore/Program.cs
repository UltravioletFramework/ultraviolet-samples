using Sample2_HandlingInput.Shared;
using Ultraviolet;

namespace Sample2_HandlingInput.NETCore
{
    public class Program : UltravioletApplication
    {
        public Program() : base("Ultraviolet", "Sample 2 - Handling Input")
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