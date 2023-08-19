using Ultraviolet;

namespace Sample1_CreatingAnApplication.NETCore
{
    public class Program : UltravioletApplication
    {
        public Program() : base("Ultraviolet", "Sample 1 - Creating an Application")
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