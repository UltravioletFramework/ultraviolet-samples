using System;
using System.IO;
using Sample2_HandlingInput.Input;
using Ultraviolet;
using Ultraviolet.BASS;
using Ultraviolet.OpenGL;

namespace Sample2_HandlingInput.Shared
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

        protected override void OnLoadingContent()
        {
            LoadInputBindings();

            base.OnLoadingContent();
        }

        protected override void OnShutdown()
        {
            SaveInputBindings();

            base.OnShutdown();
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            if (Ultraviolet.GetInput().GetActions().ExitApplication.IsPressed())
            {
                Host.Exit();
            }
            base.OnUpdating(time);
        }

        private String GetInputBindingsPath()
        {
            return Path.Combine(Host.GetRoamingApplicationSettingsDirectory(), "InputBindings.xml");
        }

        private void LoadInputBindings()
        {
            Ultraviolet.GetInput().GetActions().Load(GetInputBindingsPath(), throwIfNotFound: false);
        }

        private void SaveInputBindings()
        {
            Ultraviolet.GetInput().GetActions().Save(GetInputBindingsPath());
        }
    }
}
