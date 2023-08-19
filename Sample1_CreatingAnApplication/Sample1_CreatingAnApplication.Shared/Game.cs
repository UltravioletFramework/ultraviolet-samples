using Ultraviolet;
using Ultraviolet.BASS;
using Ultraviolet.OpenGL;

namespace Sample1_CreatingAnApplication
{
    public class Game : UltravioletApplicationAdapter
    {
        public Game(IUltravioletApplicationAdapterHost host)
            : base(host)
        { }

        protected override void OnConfiguring(UltravioletConfiguration configuration)
        {
            configuration.Plugins.Add(new OpenGLGraphicsPlugin());
            configuration.Plugins.Add(new BASSAudioPlugin());
        }
    }
}
