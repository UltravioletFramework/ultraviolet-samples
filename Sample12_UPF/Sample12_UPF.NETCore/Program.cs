using System.Linq;
using Ultraviolet;

namespace Sample12_UPF.NETCore
{
    public class Program : UltravioletApplication
    {
        private static GameFlags sFlags = GameFlags.None;
        public Program() : base("Ultraviolet", "Sample 12 - UPF")
        {
        }

        public static void Main(string[] args)
        {
            if (args.Contains("-compile:expressions"))
                sFlags |= GameFlags.CompileExpressions;

            using (var app = new Program())
            {
                app.Run();
            }
        }

        protected override UltravioletApplicationAdapter OnCreatingApplicationAdapter()
        {
            return new Game(this, sFlags);
        }

        protected override void OnInitialized()
        {
            UsePlatformSpecificFileSource();
            base.OnInitialized();
        }
    }
}