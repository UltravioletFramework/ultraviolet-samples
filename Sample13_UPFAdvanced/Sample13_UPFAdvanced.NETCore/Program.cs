using Sample13_UPFAdvanced.Shared;
using System.Linq;
using Ultraviolet;

namespace Sample13_UPFAdvanced.NETCore
{
    public class Program : UltravioletApplication
    {
        private static GameFlags sFlags = GameFlags.None;
        public Program() : base("Ultraviolet", "Sample 13 - UPF (Advanced)")
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